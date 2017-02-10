using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ProjectPracticeWeb.Models;

namespace ProjectPracticeWeb.AppCode
{
	public static class CommonMethods
	{
		public static T DeserializeJson<T>(string appSettingName)
		{
			if (string.IsNullOrEmpty(appSettingName)) return default(T);
			// wrong file path
			var path = AppDomain.CurrentDomain.BaseDirectory+ ConfigurationManager.AppSettings[appSettingName];
			if (string.IsNullOrEmpty(path)) return default(T);
			string json = null;

			using (var sReader = new StreamReader(path))
			{
				json = sReader.ReadToEnd();
			}
			var result = default(T);
			if (!string.IsNullOrEmpty(json))
			{
				result = JsonConvert.DeserializeObject<T>(json);
			}
			return result;
		}

	}
}