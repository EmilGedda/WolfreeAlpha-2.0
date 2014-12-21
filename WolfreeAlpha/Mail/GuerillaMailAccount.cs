using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WolfreeAlpha.Mail
{
	internal sealed class GuerillaMailAccount : MailAccountBase
	{
		private const string Url = "http://api.guerrillamail.com/ajax.php";

		private static readonly string[] domains =
		{
			"sharklasers.com", "grr.la", "spam4.me",
			"guerrillamailblock.com", "guerrillamail.org"
		};

		private string token;

		public GuerillaMailAccount(string firstname, string lastname)
		{
			SetMail(firstname, lastname);
			FetchEmails();
		}

		public GuerillaMailAccount()
		{
		}

		protected override string FetchAddress()
		{
			string response =
				Network.GET(String.Format("{0}?f=get_email_address&ip={1}&agent={2}", Url, Network.IP,
					HttpUtility.UrlEncode(Network.Agent)));
			var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
			token = values["sid_token"];
			return values["email_addr"];
		}

		private void SetMail(string firstname, string lastname)
		{
			if (Address == null) SetMail(firstname, lastname);
			firstname = firstname.ToLower();
			lastname = lastname.ToLower();
			string response =
				Network.GET(String.Format("{0}?f=set_email_user&sid_token={1}&email_user={2}.{3}", Url, token, firstname,
					lastname));
			var success = (bool) JsonConvert.DeserializeObject<JObject>(response)["auth"]["success"];
			if (!success) throw new Exception("Uanble to set custom email");
			string domain = domains[RandomSingleton.Instance.Next(5)];
			Address = String.Format("{0}.{1}@{2}", firstname, lastname, domain);
		}

		public override List<IBasicEmail> FetchEmails()
		{
			string response = Network.GET(String.Format("{0}?f=get_email_list&sid_token={1}&seq=0&offset=0", Url, token));
			var mailbox = JsonConvert.DeserializeObject<JObject>(response);
			var guerillaemails = mailbox["list"].ToObject<List<GuerillaEmail>>();
			foreach (GuerillaEmail guerillaemail in guerillaemails)
			{
				string mailJSON =
					Network.GET(String.Format("{0}?f=fetch_email&sid_token={1}&email_id={2}", Url, token, guerillaemail.ID));
				guerillaemail.Body = (string) JsonConvert.DeserializeObject<JToken>(mailJSON)["mail_body"];
			}

			return emails = guerillaemails.ToList<IBasicEmail>();
		}
	}
}