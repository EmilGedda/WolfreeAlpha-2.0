using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WolfreeAlpha
{
	class Network
	{
		public static string Agent
		{
			get { return agent; }	
		}

		private static CookieAwareWebClient webclient = new CookieAwareWebClient();

		private  const string agent =
			"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

		public static string GetIP()
		{
			return "83.250.10.156";
		}

		public static string GET(string url)
		{
			return webclient.DownloadString(url);
		}

		public static string POST(string url, string data)
		{
			return "";
		}
	}
}
