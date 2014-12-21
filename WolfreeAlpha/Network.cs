using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace WolfreeAlpha
{
	internal static class Network
	{
		private const string agent =
			"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

		private static CookieAwareWebClient webclient
		{
			get
			{
				return _webclient ?? (_webclient = Init());
			}
			set { _webclient = value; }
		}

		private static string ip;
		private static CookieAwareWebClient _webclient;

		public static string Agent
		{
			get { return agent; }
		}

		public static string IP
		{
			get
			{
				if (String.IsNullOrEmpty(ip))
					ip = webclient.DownloadString("http://ipz.emilgedda.se/");
				return ip;
			}
		}

		private static CookieAwareWebClient Init()
		{
			webclient = new CookieAwareWebClient();
			webclient.Headers[HttpRequestHeader.UserAgent] = Agent;
			webclient.Headers[HttpRequestHeader.Referer] = "http://www.wolframalpha.com/";
			webclient.Headers.Add("Origin", "http://www.wolframalpha.com/");
			return webclient;
		}

		public static string GET(string url)
		{
			return webclient.DownloadString(url);
		}

		public static string POST(string url, NameValueCollection data)
		{
			return Encoding.UTF8.GetString(webclient.UploadValues(url, data));
		}
	}
}