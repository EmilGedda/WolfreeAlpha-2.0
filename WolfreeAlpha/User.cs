using System;
using System.Linq;
using System.Runtime.InteropServices;
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
            email = new GuerillaMailAccount(firstname, lastname);
									
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
	    private ITemporaryMail email;
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
			return (Enumerable.Repeat(charset, length).Select((s, i) =>
						i == 0 ? Char.ToUpper(s[random.Next(s.Length)]) : s[random.Next(s.Length)]))
						.ToString();
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