﻿/* ********************************************************************************
 *
 * Copyright 2010-2013 Microsoft Corporation
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
using System.IO;
using Ionic.Zip;

namespace ElmcityUtils
{
	public static class FileUtils
	{

		public static DirectoryInfo CreateLocalDirectoryUnderCurrent(string name)
		{
			var cd = Directory.GetCurrentDirectory();
			return Directory.CreateDirectory(string.Format(@"{0}\{1}", cd, name));
		}

		public static void UnzipFromUrlToDirectory(Uri zip_url, string directory)
		{
			var zip_response = HttpUtils.FetchUrl(zip_url);
			var zs = new MemoryStream(zip_response.bytes);
			var zip = ZipFile.Read(zs);
			int exceptions = 0;
			foreach (var entry in zip.Entries)
			{
				try
				{
					entry.Extract(directory, ExtractExistingFileAction.DoNotOverwrite);
				}
				catch
				{
					exceptions += 1;
				}
			}

			if (exceptions > 0)
				GenUtils.PriorityLogMsg("exception", String.Format("UnzipFromUrlToDirectory: {0} exceptions", exceptions), null);
		}

	}
}
