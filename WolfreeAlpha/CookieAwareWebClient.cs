using System;
using System.Collections.Generic;
using System.Net;

namespace WolfreeAlpha
{
	public class CookieAwareWebClient : WebClient
	{
		public CookieAwareWebClient()
			: this(new CookieContainer())
		{
		}

		public CookieAwareWebClient(CookieContainer cookies)
		{
			CookieContainer = cookies;
		}

		public CookieContainer CookieContainer { get; set; }
		public Uri Uri { get; set; }

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);
			if (request is HttpWebRequest)
				(request as HttpWebRequest).CookieContainer = CookieContainer;

			var httpRequest = (HttpWebRequest) request;
			httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			return httpRequest;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = base.GetWebResponse(request);
			String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

			if (setCookieHeader == null) return response;
			foreach (Cookie c in GetAllCookiesFromHeader(setCookieHeader, response.ResponseUri.Host))
				CookieContainer.Add(c);
			return response;
		}

		private static IEnumerable<Cookie> GetAllCookiesFromHeader(string header, string host)
		{
			IEnumerable<string> stringList = ConvertCookieHeaderToStringList(header);
			return ConvertCookieArraysToCookieCollection(stringList, host);
		}

		private static IEnumerable<string> ConvertCookieHeaderToStringList(string strCookHeader)
		{
			string[] strCookTemp = strCookHeader.Replace("\r", "").Replace("\n", "").Split(',');
			var cookeStringList = new List<string>();

			for (int i = 0; i < strCookTemp.Length; i++)
				if (strCookTemp[i].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
					cookeStringList.Add(strCookTemp[i] + "," + strCookTemp[i++ + 1]);
				else
					cookeStringList.Add(strCookTemp[i]);

			return cookeStringList;
		}

		private static void HandleCookieNameAndValue(string nameAndValue, Cookie cookie)
		{
			if (nameAndValue == string.Empty) return;
			int firstEqual = nameAndValue.IndexOf("=", StringComparison.Ordinal);
			cookie.Name = nameAndValue.Substring(0, firstEqual);
			cookie.Value = nameAndValue.Substring(firstEqual + 1, nameAndValue.Length - firstEqual - 1);
		}

		private static void HandleEmptyCookieFields(Cookie c, string host)
		{
			if (c.Path == string.Empty) c.Path = "/";
			if (c.Domain == string.Empty) c.Domain = host;
		}

		private static void HandleCookieDomainAndPath(Cookie cookie, string cookieStr, IList<string> value, string host)
		{
			if (cookieStr.IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0)
				cookie.Path = value[1] != string.Empty ? value[1] : "/";
			if (cookieStr.IndexOf("domain", StringComparison.OrdinalIgnoreCase) >= 0)
				cookie.Domain = value[1] != string.Empty ? value[1] : host;
		}

		private static IEnumerable<Cookie> ConvertCookieArraysToCookieCollection(IEnumerable<string> cookieStringList, string host)
		{
			foreach (string t in cookieStringList)
			{
				string[] cookieParts = t.Split(';');
				var cookie = new Cookie();
				HandleCookieNameAndValue(cookieParts[0], cookie);

				foreach (string part in cookieParts)
					HandleCookieDomainAndPath(cookie, part, part.Split('='), host);

				HandleEmptyCookieFields(cookie, host);
				yield return cookie;
			}
		}
	}
}