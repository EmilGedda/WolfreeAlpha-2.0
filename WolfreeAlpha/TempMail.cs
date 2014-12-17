using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WolfreeAlpha
{
	class TempMail
	{
		private static string[] domains = { "sharklasers.com", "grr.la", "spam4.me", "guerrillamailblock.com", "guerrillamail.org" };
		private string token, address, mailbox;
		private const string url = "http://api.guerrillamail.com/ajax.php";

		public TempMail()
		{
			FetchAddress();
			FetchMail();
			SetMail("firstname", "lastname");
			//email = domains[new Random().Next(5)];
		}

		public void FetchAddress()
		{
			if (address != null) return;
			string response = Network.GET(String.Format("{0}?f=get_email_address&ip={1}&agent={2}", url, Network.IP, HttpUtility.UrlEncode(Network.Agent)));
			var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
			address = values["email_addr"];
			token = values["sid_token"];
		}

		public void SetMail(string firstname, string lastname)
		{
			firstname = firstname.ToLower();
			lastname = lastname.ToLower();
			string response = Network.GET(String.Format("{0}?f=set_email_user&sid_token={1}&email_user={2}.{3}", url, token, firstname, lastname));
			bool success = (bool)JsonConvert.DeserializeObject<JObject>(response)["auth"]["success"];
			if(!success) throw new Exception("Uanble to set custom email");
			string domain = domains[RandomSingleton.Instance.Next(5)];
			address = String.Format("{0}.{1}@{2}", firstname, lastname, domain);
		}

		public void FetchMail()
		{
			var response = Network.GET(String.Format("{0}?f=get_email_list&sid_token={1}&seq=0&offset=0", url, token));
			var mailbox = JsonConvert.DeserializeObject<JObject>(response)["list"];
		}
	}
}
