﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace ElmcityUtils
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class iis_log_entry_entities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new iis_log_entry_entities object using the connection string found in the 'iis_log_entry_entities' section of the application configuration file.
        /// </summary>
        public iis_log_entry_entities() : base("name=iis_log_entry_entities", "iis_log_entry_entities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new iis_log_entry_entities object.
        /// </summary>
        public iis_log_entry_entities(string connectionString) : base(connectionString, "iis_log_entry_entities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new iis_log_entry_entities object.
        /// </summary>
        public iis_log_entry_entities(EntityConnection connection) : base(connection, "iis_log_entry_entities")
        {
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<iis_log_entry> iis_log_entry
        {
            get
            {
                if ((_iis_log_entry == null))
                {
                    _iis_log_entry = base.CreateObjectSet<iis_log_entry>("iis_log_entry");
                }
                return _iis_log_entry;
            }
        }
        private ObjectSet<iis_log_entry> _iis_log_entry;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the iis_log_entry EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToiis_log_entry(iis_log_entry iis_log_entry)
        {
            base.AddObject("iis_log_entry", iis_log_entry);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="iis_log_entry", Name="iis_log_entry")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class iis_log_entry : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new iis_log_entry object.
        /// </summary>
        /// <param name="id">Initial value of the ID property.</param>
        /// <param name="datetime">Initial value of the datetime property.</param>
        /// <param name="server">Initial value of the server property.</param>
        /// <param name="verb">Initial value of the verb property.</param>
        /// <param name="url">Initial value of the url property.</param>
        /// <param name="ip">Initial value of the ip property.</param>
        /// <param name="status">Initial value of the status property.</param>
        /// <param name="sent_bytes">Initial value of the sent_bytes property.</param>
        /// <param name="recv_bytes">Initial value of the recv_bytes property.</param>
        /// <param name="time_taken">Initial value of the time_taken property.</param>
        public static iis_log_entry Createiis_log_entry(global::System.Int32 id, global::System.DateTime datetime, global::System.String server, global::System.String verb, global::System.String url, global::System.String ip, global::System.Int32 status, global::System.Int32 sent_bytes, global::System.Int32 recv_bytes, global::System.Int32 time_taken)
        {
            iis_log_entry iis_log_entry = new iis_log_entry();
            iis_log_entry.ID = id;
            iis_log_entry.datetime = datetime;
            iis_log_entry.server = server;
            iis_log_entry.verb = verb;
            iis_log_entry.url = url;
            iis_log_entry.ip = ip;
            iis_log_entry.status = status;
            iis_log_entry.sent_bytes = sent_bytes;
            iis_log_entry.recv_bytes = recv_bytes;
            iis_log_entry.time_taken = time_taken;
            return iis_log_entry;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int32 _ID;
        partial void OnIDChanging(global::System.Int32 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime datetime
        {
            get
            {
                return _datetime;
            }
            set
            {
                OndatetimeChanging(value);
                ReportPropertyChanging("datetime");
                _datetime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("datetime");
                OndatetimeChanged();
            }
        }
        private global::System.DateTime _datetime;
        partial void OndatetimeChanging(global::System.DateTime value);
        partial void OndatetimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String server
        {
            get
            {
                return _server;
            }
            set
            {
                OnserverChanging(value);
                ReportPropertyChanging("server");
                _server = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("server");
                OnserverChanged();
            }
        }
        private global::System.String _server;
        partial void OnserverChanging(global::System.String value);
        partial void OnserverChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String verb
        {
            get
            {
                return _verb;
            }
            set
            {
                OnverbChanging(value);
                ReportPropertyChanging("verb");
                _verb = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("verb");
                OnverbChanged();
            }
        }
        private global::System.String _verb;
        partial void OnverbChanging(global::System.String value);
        partial void OnverbChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String url
        {
            get
            {
                return _url;
            }
            set
            {
                OnurlChanging(value);
                ReportPropertyChanging("url");
                _url = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("url");
                OnurlChanged();
            }
        }
        private global::System.String _url;
        partial void OnurlChanging(global::System.String value);
        partial void OnurlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String ip
        {
            get
            {
                return _ip;
            }
            set
            {
                OnipChanging(value);
                ReportPropertyChanging("ip");
                _ip = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("ip");
                OnipChanged();
            }
        }
        private global::System.String _ip;
        partial void OnipChanging(global::System.String value);
        partial void OnipChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String http_version
        {
            get
            {
                return _http_version;
            }
            set
            {
                Onhttp_versionChanging(value);
                ReportPropertyChanging("http_version");
                _http_version = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("http_version");
                Onhttp_versionChanged();
            }
        }
        private global::System.String _http_version;
        partial void Onhttp_versionChanging(global::System.String value);
        partial void Onhttp_versionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String user_agent
        {
            get
            {
                return _user_agent;
            }
            set
            {
                Onuser_agentChanging(value);
                ReportPropertyChanging("user_agent");
                _user_agent = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("user_agent");
                Onuser_agentChanged();
            }
        }
        private global::System.String _user_agent;
        partial void Onuser_agentChanging(global::System.String value);
        partial void Onuser_agentChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String referrer
        {
            get
            {
                return _referrer;
            }
            set
            {
                OnreferrerChanging(value);
                ReportPropertyChanging("referrer");
                _referrer = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("referrer");
                OnreferrerChanged();
            }
        }
        private global::System.String _referrer;
        partial void OnreferrerChanging(global::System.String value);
        partial void OnreferrerChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 status
        {
            get
            {
                return _status;
            }
            set
            {
                OnstatusChanging(value);
                ReportPropertyChanging("status");
                _status = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("status");
                OnstatusChanged();
            }
        }
        private global::System.Int32 _status;
        partial void OnstatusChanging(global::System.Int32 value);
        partial void OnstatusChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> w32_status
        {
            get
            {
                return _w32_status;
            }
            set
            {
                Onw32_statusChanging(value);
                ReportPropertyChanging("w32_status");
                _w32_status = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("w32_status");
                Onw32_statusChanged();
            }
        }
        private Nullable<global::System.Int32> _w32_status;
        partial void Onw32_statusChanging(Nullable<global::System.Int32> value);
        partial void Onw32_statusChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 sent_bytes
        {
            get
            {
                return _sent_bytes;
            }
            set
            {
                Onsent_bytesChanging(value);
                ReportPropertyChanging("sent_bytes");
                _sent_bytes = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("sent_bytes");
                Onsent_bytesChanged();
            }
        }
        private global::System.Int32 _sent_bytes;
        partial void Onsent_bytesChanging(global::System.Int32 value);
        partial void Onsent_bytesChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 recv_bytes
        {
            get
            {
                return _recv_bytes;
            }
            set
            {
                Onrecv_bytesChanging(value);
                ReportPropertyChanging("recv_bytes");
                _recv_bytes = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("recv_bytes");
                Onrecv_bytesChanged();
            }
        }
        private global::System.Int32 _recv_bytes;
        partial void Onrecv_bytesChanging(global::System.Int32 value);
        partial void Onrecv_bytesChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 time_taken
        {
            get
            {
                return _time_taken;
            }
            set
            {
                Ontime_takenChanging(value);
                ReportPropertyChanging("time_taken");
                _time_taken = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("time_taken");
                Ontime_takenChanged();
            }
        }
        private global::System.Int32 _time_taken;
        partial void Ontime_takenChanging(global::System.Int32 value);
        partial void Ontime_takenChanged();

        #endregion
    
    }

    #endregion
    
}
