using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

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

		public static byte[] GetHash(string inputString)
		{
			HashAlgorithm algorithm = MD5.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}

		public static string GetHashString(string inputString)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in GetHash(inputString))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		public static bool EqualsState(object objectToCompare, IState thisObject)
		{
			if (objectToCompare == null || objectToCompare.GetType() != thisObject.GetType())
			{
				return false;
			}

			var state = (IState)objectToCompare;

			return state.NameOfState == thisObject.NameOfState;
		}
	}
}