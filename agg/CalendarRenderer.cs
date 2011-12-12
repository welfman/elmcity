﻿/* ********************************************************************************
 *
 * Copyright 2010 Microsoft Corporation
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
 * Unless required by applicable law or agreed to in writing, software distributed 
 * under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License. 
 *
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ElmcityUtils;
using Newtonsoft.Json;

namespace CalendarAggregator
{

	// render calendar data in various formats

	[Serializable]
	public class CalendarRenderer
	{
		private string id;
		private Calinfo calinfo;
		//private BlobStorage bs = BlobStorage.MakeDefaultBlobStorage();
		private string template_html;

		public string xmlfile
		{
			get { return _xmlfile; }
			set { _xmlfile = value; }
		}
		private string _xmlfile;

		public string jsonfile
		{
			get { return _jsonfile; }
			set { _jsonfile = value; }
		}
		private string _jsonfile;

		public string htmlfile
		{
			get { return _htmlfile; }
			set { _htmlfile = value; }
		}
		private string _htmlfile;

		// data might be available in cache,
		// this interface abstracts the cache so its logic can be tested
		public ICache cache
		{
			get { return _cache; }
			set { _cache = value; }
		}
		private ICache _cache;

		// points to a method for rendering individual events in various formats
		private delegate string EventRenderer(ZonelessEvent evt, Calinfo calinfo);

		// points to a method for rendering views of events in various formats
		public delegate string ViewRenderer(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to);

		// points to a method for retrieving a pickled event store
		// normally used with caching: es_getter = new EventStoreGetter(GetEventStoreWithCaching);
		// but could bypass cache: es_getter = new EventStoreGetter(GetEventStoreWithoutCaching);
		// returns a ZonelessEventStore
		private delegate ZonelessEventStore EventStoreGetter(ICache cache);

		private EventStoreGetter es_getter;

		public const string DATETIME_FORMAT_FOR_XML = "yyyy-MM-ddTHH:mm:ss";

		// used by the service in both WorkerRole and WebRole
		// in WorkerRole: when saving renderings
		// in WebRole: when serving renderings
		public CalendarRenderer(string id)
		{
			this.calinfo = Utils.AcquireCalinfo(id);
			this.cache = null;
			this.es_getter = new EventStoreGetter(GetEventStoreWithCaching);
			try
			{
				this.id = id;

				try
				{
					this.template_html = HttpUtils.FetchUrl(calinfo.template_url).DataAsString();
				}
				catch (Exception e)
				{
					GenUtils.PriorityLogMsg("exception", "CalendarRenderer: cannot fetch template", e.InnerException.Message);
					throw (e);
				}

				this.xmlfile = this.id + ".xml";
				this.jsonfile = this.id + ".json";
				this.htmlfile = this.id + ".html";

				//  this.ical_sources = Collector.GetIcalSources(this.id);
			}
			catch (Exception e)
			{
				GenUtils.PriorityLogMsg("exception", "CalenderRenderer.CalendarRenderer: " + id, e.Message + e.StackTrace);
			}

		}

		#region xml

		// used by WorkerRole to save current xml rendering to azure blob
		public string SaveAsXml()
		{
			var bs = BlobStorage.MakeDefaultBlobStorage();
			string xml = "";
			xml = this.RenderXml(0);
			byte[] bytes = Encoding.UTF8.GetBytes(xml.ToString());
			//BlobStorage.WriteToAzureBlob(this.bs, this.id, this.xmlfile, "text/xml", bytes);
			bs.PutBlob(this.id, this.xmlfile, xml.ToString(), "text/xml");
			return xml.ToString();
		}

		public string RenderXml()
		{
			return RenderXml(eventstore: null, view: null, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderXml(string view)
		{
			return RenderXml(eventstore: null, view: view, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderXml(int count)
		{
			return RenderXml(eventstore: null, view: null, count: count, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		// render an eventstore as xml, optionally limited by view and/or count
		public string RenderXml(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to)
		{
			eventstore = GetEventStore(eventstore, view, count, from, to);

			var xml = new StringBuilder();
			xml.Append("<events>\n");

			var event_renderer = new EventRenderer(RenderEvtAsXml);

			var eventstring = new StringBuilder();

			foreach (var evt in eventstore.events)
				AppendEvent(eventstring, event_renderer, evt);

			xml.Append(eventstring.ToString());

			xml.Append("</events>\n");

			return xml.ToString();
		}

		// render a single event as an xml element
		private string RenderEvtAsXml(ZonelessEvent evt, Calinfo calinfo)
		{
			var xml = new StringBuilder();
			xml.Append("<event>\n");
			xml.Append(string.Format("<title>{0}</title>\n", HttpUtility.HtmlEncode(evt.title)));
			xml.Append(string.Format("<url>{0}</url>\n", HttpUtility.HtmlEncode(evt.url)));
			xml.Append(string.Format("<source>{0}</source>\n", HttpUtility.HtmlEncode(evt.source)));
			xml.Append(string.Format("<dtstart>{0}</dtstart>\n", evt.dtstart.ToString(DATETIME_FORMAT_FOR_XML)));
			if (evt.dtend != null)
				xml.Append(string.Format("<dtend>{0}</dtend>\n", evt.dtend.ToString(DATETIME_FORMAT_FOR_XML)));
			xml.Append(string.Format("<allday>{0}</allday>\n", evt.allday));
			xml.Append(string.Format("<categories>{0}</categories>\n", HttpUtility.HtmlEncode(evt.categories)));
			xml.Append(string.Format("<description>{0}</description>\n", HttpUtility.HtmlEncode(evt.description)));
			//if (this.calinfo.hub_type == HubType.where.ToString())
			if (calinfo.hub_enum == HubType.where)
			{
				var lat = evt.lat != null ? evt.lat : this.calinfo.lat;
				var lon = evt.lon != null ? evt.lon : this.calinfo.lon;
				xml.Append(string.Format("<lat>{0}</lat>\n", lat));
				xml.Append(string.Format("<lon>{0}</lon>\n", lon));
			}
			xml.Append("</event>\n");
			return xml.ToString();
		}

		#endregion xml

		#region json

		public BlobStorageResponse SaveAsJson()
		{
			var es = new ZonelessEventStore(this.calinfo).Deserialize();
			var json = JsonConvert.SerializeObject(es.events);
			return Utils.SerializeObjectToJson(es.events, this.id, this.jsonfile);
		}

		public string RenderJson()
		{
			return RenderJson(eventstore: null, view: null, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderJson(string view)
		{
			return RenderJson(eventstore: null, view: view, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderJson(int count)
		{
			return RenderJson(eventstore: null, view: null, count: count, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderJson(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to)
		{
			eventstore = GetEventStore(eventstore, view, count, from, to);
			for (var i = 0; i < eventstore.events.Count; i++)
			{
				var evt = eventstore.events[i];
				//if (this.calinfo.hub_type == HubType.where.ToString())
				if ( this.calinfo.hub_enum == HubType.where )
				{
					evt.lat = evt.lat != null ? evt.lat : this.calinfo.lat;
					evt.lon = evt.lon != null ? evt.lon : this.calinfo.lon;
				}
				// provide utc so browsers receiving the json don't apply their own timezones
				evt = ZonelessEventStore.UniversalFromLocalAndTzinfo(evt, this.calinfo.tzinfo);
				eventstore.events[i] = evt;
			}
			return JsonConvert.SerializeObject(eventstore.events);
		}

		#endregion json

		#region html

		public string SaveAsHtml()
		{
			string html = this.RenderHtml();
			byte[] bytes = Encoding.UTF8.GetBytes(html);
			//BlobStorage.WriteToAzureBlob(this.bs, this.id, this.htmlfile, "text/html", bytes);
			var bs = BlobStorage.MakeDefaultBlobStorage();
			bs.PutBlob(this.id, this.htmlfile, html, "text/html");
			return html;
		}

		public string RenderHtml()
		{
			return RenderHtml(eventstore: null, view: null, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderHtml(ZonelessEventStore es)
		{
			return RenderHtml(eventstore: es, view: null, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderHtml(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to)
		{
			eventstore = GetEventStore(eventstore, view, count, from, to);

			var builder = new StringBuilder();

			RenderEventsAsHtml(eventstore, builder, announce_time_of_day: true);

			var html = this.template_html.Replace("__EVENTS__", builder.ToString());

			html = html.Replace("__ID__", this.id);
			html = html.Replace("__CSSURL__", this.calinfo.css);
			html = html.Replace("__TITLE__", this.calinfo.title);
			html = html.Replace("__CONTRIBUTE__", this.calinfo.contribute_url.ToString());

			html = html.Replace("__WIDTH__", this.calinfo.display_width);

			if (this.calinfo.has_img)
			{
				html = html.Replace("__IMG__", this.calinfo.default_img_html);
				html = html.Replace("__IMGURL__", this.calinfo.img_url.ToString());
			}
			else
			{
				html = html.Replace("__IMG__", "");
			}

			html = html.Replace("__CONTACT__", this.calinfo.contact);

			var sources_builder = new StringBuilder();

			if (this.calinfo.eventful)
				sources_builder.Append(@"<div class=""sources""><img alt=""Local Events, Concerts, Tickets"" src=""http://elmcity.blob.core.windows.net/admin/eventful_logo.gif""><p><a href=""http://eventful.com/"">Events</a> by Eventful</p></div>");

			if (this.calinfo.upcoming)
				sources_builder.Append(@"<p><a href=""http://upcoming.yahoo.com""><img width=""180"" src=""http://elmcity.blob.core.windows.net/admin/upcoming_logo.gif""></a></p>");

			if (this.calinfo.eventbrite)
				sources_builder.Append(@"<p><a href=""http://eventbrite.com""><img height=""50"" src=""http://elmcity.blob.core.windows.net/admin/eventbrite_logo.jpg""></a></p>");

			if (this.calinfo.facebook)
				sources_builder.Append(@"<p class=""sources"" style=""font-size:larger""><a href=""http://facebook.com"">facebook</a></p>");

			var ical_feeds = string.Format(@"<p class=""sources""><a target=""_new"" href=""http://elmcity.cloudapp.net/services/{0}/stats"">{1} iCalendar feeds</a></p>",
				this.calinfo.id, this.calinfo.feed_count);
			sources_builder.Append(ical_feeds);

			html = html.Replace("__SOURCES__", sources_builder.ToString());
			string generated = String.Format("{0}\n{1}\n{2}\n{3}",
					System.DateTime.UtcNow.ToString(),
					System.Net.Dns.GetHostName(),
					JsonConvert.SerializeObject(GenUtils.GetSettingsFromAzureTable("usersettings")),
					JsonConvert.SerializeObject(this.calinfo) );
			html = html.Replace("__GENERATED__", generated);

			return html;
		}

		// the default html rendering chunks by day, this method processes the raw list of events into
		// the ZonelessEventStore's event_dict like so:
		// key: d20100710
		// value: [ <ZonelessEvent>, <ZonelessEvent> ... ]
		public static void OrganizeByDate(ZonelessEventStore es)
		{
			es.GroupEventsByDatekey();
			es.SortEventSublists();
			es.SortDatekeys();
		}

		private void RenderEventsAsHtml(ZonelessEventStore es, StringBuilder builder, bool announce_time_of_day)
		{
			OrganizeByDate(es);
			TimeOfDay current_time_of_day;
			var event_renderer = new EventRenderer(RenderEvtAsHtml);
			var year_month_anchors = new List<string>(); // e.g. ym201110
			foreach (string datekey in es.datekeys)
			{
				var year_month_anchor = datekey.Substring(1, 6);
				if (!year_month_anchors.Exists(ym => ym == year_month_anchor))
				{
					builder.Append(string.Format("\n<a name=\"ym{0}\"></a>\n", year_month_anchor));
					year_month_anchors.Add(year_month_anchor);
				}
				current_time_of_day = TimeOfDay.Initialized;
				var event_builder = new StringBuilder();
				var date = Utils.DateFromDateKey(datekey);
				event_builder.Append(string.Format("\n<a name=\"{0}\"></a>\n", datekey));
				event_builder.Append(string.Format("<h1 class=\"eventDate\"><b>{0}</b></h1>\n", date));
				foreach (ZonelessEvent evt in es.event_dict[datekey])
				{

					if (announce_time_of_day)
						// see http://blog.jonudell.net/2009/06/18/its-the-headings-stupid/
						MaybeAnnounceTimeOfDay(event_builder, ref current_time_of_day, evt.dtstart);
					AppendEvent(event_builder, event_renderer, evt);
				}
				builder.Append(event_builder);
			}
		}

		public string RenderEvtAsHtml(ZonelessEvent evt, Calinfo calinfo)
		{
			if (evt.urls_and_sources == null)                                                             
				evt.urls_and_sources = new Dictionary<string, string>() { { evt.url, evt.source } };

			//if (evt.list_of_urls_and_sources == null)
			//	evt.list_of_urls_and_sources = new List<List<string>>();

			string dtstart;
			 if (evt.allday)
				dtstart = "  ";
			else
				dtstart = evt.dtstart.ToString("ddd hh:mm tt  ");

			//string evt_title;
			//evt_title = MakeTitleForRDFa(evt);

			string categories = "";
			List<string> catlist_links = new List<string>();
			if (!String.IsNullOrEmpty(evt.categories))
			{
				List<string> catlist = evt.categories.Split(',').ToList();
				foreach (var cat in catlist.Unique())
				{
					var category_url = string.Format("/{0}/html?view={1}", this.id, cat);
					catlist_links.Add(string.Format(@"<a href=""{0}"">{1}</a>", category_url, cat));
				}
				categories = string.Format(@" <span class=""cat"">{0}</span>", string.Join(", ", catlist_links.ToArray()));
			}

			return string.Format(
@"
<div class=""bl"" xmlns:v=""http://rdf.data-vocabulary.org/#"" typeof=""v:Event"" >
<span class=""st"" property=""v:startDate"" content=""{0}"">{1}</span> 
<span href=""{2}"" rel=""v:url""></span>
<span class=""ttl"">{3}</span> 
<span class=""src"" property=""v:description"">{4}</span> {5} 
{6}
</div>",
			String.Format("{0:yyyy-MM-ddTHH:mm}", evt.dtstart),
			dtstart,
			evt.url,
			MakeTitleForRDFa(evt),
			//evt.list_of_urls_and_sources.Count == 1 ? evt.source : "", // suppress source if multiple
			evt.urls_and_sources.Keys.Count == 1 ? evt.source : "", // suppress source if multiple
			categories,
			MakeGeoForRDFa(evt)
			);
		}

		public static string MakeTitleForRDFa(ZonelessEvent evt)
		{
			if (evt.urls_and_sources.Keys.Count == 1)
			{
				return string.Format("<a target=\"{0}\" property=\"v:summary\" title=\"{1}\" href=\"{2}\">{3}</a>",
					Configurator.default_html_window_name, 
					evt.source,
					evt.url, 
					evt.title);
			}

			if (evt.urls_and_sources.Keys.Count > 1)
			{
				var evt_title = @"<span property=""v:summary"">" + evt.title + "</span> [";
				int count = 0;
				foreach (var url in evt.urls_and_sources.Keys )
				{
					var source = evt.urls_and_sources[url];
					count++;
					evt_title += string.Format(@"<a target=""{0}"" title=""{1}"" href=""{2}"">&nbsp;{3}&nbsp;</a>",
						Configurator.default_html_window_name,
						source,
						url,
						count);
				}
				evt_title += "]";
				return evt_title;
			}
			GenUtils.PriorityLogMsg("warning", "MakeTitleForRDFa: no title", null);
			return "";
		}

		private string MakeGeoForRDFa(ZonelessEvent evt)
		{
			string geo = "";
			if (this.calinfo.hub_enum == HubType.where)
				geo = string.Format(
@"<span rel=""v:location"">
    <span rel=""v:geo"">
       <span typeof=""v:Geo"">
          <span property=""v:latitude"" content=""{0}"" ></span>
          <span property=""v:longitude"" content=""{1}"" ></span>
       </span>
    </span>
  </span>",
				evt.lat != null ? evt.lat : this.calinfo.lat,
				evt.lon != null ? evt.lon : this.calinfo.lon
				);
			return geo;
		}

		// just today's events, used by, e.g., RenderJsWidget
		public string RenderTodayAsHtml()
		{
			ZonelessEventStore es = FindTodayEvents();
			var sb = new StringBuilder();
			foreach (var evt in es.events)
				sb.Append(RenderEvtAsHtml(evt, this.calinfo));
			return sb.ToString();
		}

		#endregion html

		#region ics

		public string RenderIcs()
		{
			return RenderIcs(eventstore: null, view: null, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderIcs(string view)
		{
			return RenderIcs(eventstore: null, view: view, count: 0, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderIcs(int count)
		{
			return RenderIcs(eventstore: null, view: null, count: count, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderIcs(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to)
		{
			eventstore = GetEventStore(eventstore, view, count, from, to);
			var ical = new DDay.iCal.iCalendar();
			Collector.AddTimezoneToDDayICal(ical, this.calinfo.tzinfo);
			var tzid = this.calinfo.tzinfo.Id;

			foreach (var evt in eventstore.events)
			{
				var ical_evt = new DDay.iCal.Event();
				ical_evt.Start = new DDay.iCal.iCalDateTime(evt.dtstart);
				ical_evt.Start.TZID = tzid;
				ical_evt.End = new DDay.iCal.iCalDateTime(evt.dtend);
				ical_evt.End.TZID = tzid;
				ical_evt.Summary = evt.title;
				ical_evt.Url = new Uri(evt.url);

				if (evt.description != null)
					ical_evt.Description = evt.description + " " + evt.url;
				else
					ical_evt.Description = evt.url;

				ical_evt.Description = evt.description;
				Collector.AddCategoriesFromCatString(ical_evt, evt.categories);
				ical.Children.Add(ical_evt);
			}

			var ics_text = Utils.SerializeIcalToIcs(ical);

			return ics_text;
		}

		#endregion ics

		#region rss

		public string RenderRss()
		{
			return RenderRss(eventstore: null, view: null, count: Configurator.rss_default_items, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderRss(int count)
		{
			return RenderRss(eventstore: null, view: null, count: count, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderRss(string view)
		{
			return RenderRss(eventstore: null, view: view, count: Configurator.rss_default_items, from: DateTime.MinValue, to: DateTime.MinValue);
		}

		public string RenderRss(ZonelessEventStore eventstore, string view, int count, DateTime from, DateTime to)
		{
			try
			{
				eventstore = GetEventStore(eventstore, view, count, from, to);
				var query = string.Format("view={0}&count={1}", view, count);
				return Utils.RssFeedFromEventStore(this.id, query, eventstore);
			}
			catch (Exception e)
			{
				GenUtils.PriorityLogMsg("exception", String.Format("RenderRss: view {0}, count {1}", view, count), e.Message);
				return String.Empty;
			}
		}

		#endregion rss

		#region tags

		public string RenderTagCloudAsHtml()
		{
			var tagcloud = MakeTagCloud(); // see http://blog.jonudell.net/2009/09/16/familiar-idioms/
			var html = new StringBuilder("<h1>tags for " + this.id + "</h1>");
			html.Append(@"<table cellpadding=""6"">");
			foreach (var pair in tagcloud)
			{
				var key = pair.Keys.First();
				html.Append("<tr>");
				html.Append("<td>" + key + "</td>");
				html.Append(@"<td align=""right"">" + pair[key] + "</td>");
				html.Append("</tr>");
			}
			html.Append("</table>");
			return html.ToString();
		}

		public string RenderTagCloudAsJson()
		{
			var tagcloud = MakeTagCloud();
			return JsonConvert.SerializeObject(tagcloud);
		}

		// see http://blog.jonudell.net/2009/09/16/familiar-idioms/
		private List<Dictionary<string, int>> MakeTagCloud()
		{
			var es = this.es_getter(this.cache);
			var tagquery =
				from evt in es.events
				where evt.categories != null
				from tag in evt.categories.Split(',')
				where tag != ""
				group tag by tag into occurrences
				orderby occurrences.Count() descending
				select new Dictionary<string, int>() { { occurrences.Key, occurrences.Count() } };
			return tagquery.ToList();
		}

		#endregion

		private void MaybeAnnounceTimeOfDay(StringBuilder eventstring, ref TimeOfDay current_time_of_day, DateTime dt)
		{
			if (this.calinfo.hub_enum == HubType.what)
				return;

			if (Utils.ClassifyTime(dt) != current_time_of_day)
			{
				TimeOfDay new_time_of_day = Utils.ClassifyTime(dt);
				string tod;
				if (new_time_of_day != TimeOfDay.AllDay)
					tod = new_time_of_day.ToString();
				else
					tod = Configurator.ALL_DAY;
				eventstring.Append(string.Format(@"<h2 class=""timeofday"">{0}</h2>", tod));
				current_time_of_day = new_time_of_day;
			}
		}

		private ZonelessEventStore GetEventStore(ZonelessEventStore es, string view, int count, DateTime from, DateTime to)
		{
			// see RenderDynamicViewWithCaching:
			// view_data = Encoding.UTF8.GetBytes(view_renderer(eventstore:null, view:view, view:count));
			// the renderer might be, e.g., CalendarRenderer.RenderHtml, which calls this method
			if (es == null)  // if no eventstore passed in (e.g., for testing)
				es = this.es_getter(this.cache); // get the eventstore. if the getter is GetEventStoreWithCaching
			// then it will use HttpUtils.RetrieveBlobFromServerCacheOrUri
			// which gets from cache if it can, else fetches uri and loads cache
			es.events = Filter(view, count, from, to, es); // then filter if requested
			return es;
		}

		// take a string representation of a set of events, in some format
		// take a per-event renderer for that format
		// take an event object
		// call the renderer to add the event object to the string representation
		// currently uses: RenderEvtAsHtml, RenderEvtAsXml
		private void AppendEvent(StringBuilder eventstring, EventRenderer event_renderer, ZonelessEvent evt)
		{
			eventstring.Append(event_renderer(evt, this.calinfo));
		}

		// produce a javascript version of today's events, for
		// inclusion on a site using <script src="">
		public string RenderJsWidget()
		{
			ZonelessEventStore es = FindTodayEvents();
			var html = new StringBuilder();
			foreach (var evt in es.events)
				html.Append(RenderEvtAsHtml(evt, this.calinfo));
			html = html.Replace(@"<h3 class=""eventBlurb""", @"<p class=""eventBlurb""");
			html = html.Replace("</h3>", "</p>");
			html = html.Replace("\'", "\\\'").Replace("\"", "\\\"");
			html = html.Replace(Environment.NewLine, "");
			return (string.Format("document.write('{0}')", html));
		}

		// return an eventstore with just today's events for this hub
		public ZonelessEventStore FindTodayEvents()
		{
			ZonelessEventStore es;
			try
			{
				es = this.es_getter(this.cache);
				es.events = es.events.FindAll(e => Utils.DtIsTodayInTz(e.dtstart, this.calinfo.tzinfo));
				var events_having_dt = es.events.FindAll(evt => ZonelessEventStore.IsZeroHourMinSec(evt) == false);
				var events_not_having_dt = es.events.FindAll(evt => ZonelessEventStore.IsZeroHourMinSec(evt) == true);
				es.events = new List<ZonelessEvent>();
				foreach (var evt in events_having_dt)
					es.events.Add(evt);
				foreach (var evt in events_not_having_dt)
					es.events.Add(evt);
			}
			catch (Exception e)
			{
				es = new ZonelessEventStore(this.calinfo);
				GenUtils.PriorityLogMsg("exception", "CalendarRenderer.FindTodayEvents", e.Message + e.StackTrace);
			}
			return es;
		}

		// possibly filter an event list by view or count
		public List<ZonelessEvent> Filter(string view, int count, DateTime from, DateTime to, ZonelessEventStore es)
		{
			var events = es.events;

			if ( from != DateTime.MinValue )
				events = events.FindAll(evt => evt.dtstart >= from && evt.dtstart <= to);  // reduce to time window

			if (view != null) // reduce to matching categories
			{
				var view_list = view.Split(',').ToList();
				foreach (var view_item in view_list)
				{
					var item = view_item.Trim(' ');
					events = events.FindAll(evt => evt.categories != null && evt.categories.ToLower().Contains(item));
				}
			}
			if (count != 0)   // reduce to first count events
				events = events.Take(count).ToList();
			return events;
		}

		// the CalendarRenderer object uses this to get the pickled object that contains an eventstore,
		// from the CalendarRender's cache if available, else fetching bytes
		private ZonelessEventStore GetEventStoreWithCaching(ICache cache)
		{
			var es = new ZonelessEventStore(this.calinfo);
			var obj_bytes = (byte[])CacheUtils.RetrieveBlobAndEtagFromServerCacheOrUri(cache, es.uri)["response_body"];
			return (ZonelessEventStore)BlobStorage.DeserializeObjectFromBytes(obj_bytes);
		}

		private ZonelessEventStore GetEventStoreWithoutCaching(ICache cache)
		{
			var es = new ZonelessEventStore(this.calinfo);
			var obj_bytes = HttpUtils.FetchUrl(es.uri).bytes;
			return (ZonelessEventStore)BlobStorage.DeserializeObjectFromBytes(obj_bytes);
		}

		// used in WebRole for views built from pickled objects that are cached
		public string RenderDynamicViewWithCaching(ControllerContext context, string view_key, ViewRenderer view_renderer, string view, int count, DateTime from, DateTime to)
		{
			try
			{
				var view_is_cached = this.cache[view_key] != null;
				byte[] view_data;
				byte[] response_body;
				if (view_is_cached)
					view_data = (byte[])cache[view_key];
				else
					view_data = Encoding.UTF8.GetBytes(view_renderer(eventstore: null, view: view, count: count, from: from, to: to));

				response_body = CacheUtils.MaybeSuppressResponseBodyForView(cache, context, view_data);
				return Encoding.UTF8.GetString(response_body);
			}
			catch (Exception e)
			{
				GenUtils.PriorityLogMsg("exception", "RenderDynamicViewWithCaching: " + view_key, e.Message + e.StackTrace);
				return RenderDynamicViewWithoutCaching(context, view_renderer, view, count, from, to);
			}
		}

		public string RenderDynamicViewWithoutCaching(ControllerContext context, ViewRenderer view_renderer, String view, int count, DateTime from, DateTime to)
		{
			ZonelessEventStore es = GetEventStoreWithCaching(this.cache);
			return view_renderer(es, view, count, from, to);
		}

	}
}
