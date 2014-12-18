using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.ModelBinding;
using WolfreeAlpha.Mail;

namespace WolfreeAlpha
{
	internal class User
	{
		private User(string firstname, string lastname)
		{
			FirstName = firstname;
			LastName = lastname;
            const string passwordCharset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Password = RandomString(RandomSingleton.Instance, passwordCharset);
            mailAccount = new GuerillaMailAccount(firstname, lastname);
									
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
	    private readonly ITemporaryMail mailAccount;
		public ITemporaryMail EmailAccount { get { return mailAccount; } }
		public string FullName
		{
			get { return String.Concat(FirstName, " ", LastName); }
		}

		public static User CreateRandomUser()
		{
			const string nameCharSet = "abcdefghijklmnopqrstuvwxyz";
			return new User(RandomString(RandomSingleton.Instance, nameCharSet),
							RandomString(RandomSingleton.Instance, nameCharSet));
		}

		public static User CreateRealisticUser()
		{
			throw new NotImplementedException();
		}
		private static string RandomString(Random random, string charset, int length = 8)
		{
			return new string(Enumerable.Repeat(charset, length).Select((s, i) =>
						i == 0 ? Char.ToUpper(s[random.Next(s.Length)]) : s[random.Next(s.Length)])
						.ToArray());
		}

		public void CreateAccount()
		{
			Network.GET("http://www.wolframalpha.com/");
			NameValueCollection data = new NameValueCollection
			{
				{"email", EmailAccount.Address},
				{"firstname", FirstName},
				{"lastname", LastName},
				{"password", Password},
				{"passwordc", Password},
				{"referer", ""},
			};
			string response = Network.POST("http://www.wolframalpha.com/input/signup.jsp", data);
		}
		public void StartProTrial()
		{
			TimeSpan timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
			Network.GET(
				"http://www.wolframalpha.com/click.txt?action=starttrial&src=img-ad&location=http://www.wolframalpha.com/&ts=" +
				timestamp.TotalSeconds);
			NameValueCollection data = new NameValueCollection
			{
				{"immediateresult", ""},
				{"confirmtrial", ""},
			};
			Network.POST("http://www.wolframalpha.com/input/trial.jsp", data);
		}
		public void VerifyAccount()
		{
			string body =
				EmailAccount.CachedEmails.First(e => e.Subject == "Wolfram Alpha Pro email verification")
					.Body.Replace("\n", "")
					.Replace("\r", " ");

			string url = Regex.Match(body, "((?:http|https)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\"]*))").Value;
			Network.GET(url);
		}
		public static explicit operator string(User user)
		{
			return user.ToString();
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}