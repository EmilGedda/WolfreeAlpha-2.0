using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace WolfreeAlpha
{
	class TempMail
	{
		private static string[] domains = { "sharklasers.com", "grr.la", "spam4.me", "guerrillamailblock.com", "guerrillamail.org" };
		private string token, timestamp, addr, email;
		private const string url = "http://api.guerrillamail.com/ajax.php";

		public TempMail()
		{
			email = domains[new Random().Next(5)];
		}

		public void FetchAddress()
		{
			string response = Network.GET(url + "?f=get_email_address&ip=" + Network.GetIP() + "&agent=" + HttpUtility.UrlEncode(Network.Agent));
		}
	}
}
