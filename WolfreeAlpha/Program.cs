using System;
using System.Threading;

namespace WolfreeAlpha
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				User user = User.CreateRandomUser();
				user.CreateAccount();
				for (int i = 0; !user.EmailAccount.HasRegistrationMail(); i++)
				{
					user.EmailAccount.FetchEmails();
					Thread.Sleep(100);
					if (i == 200)
						throw new Exception("Unable to create account! No registration email found.");
				}
				user.StartProTrial();
				for (int i = 0; !user.EmailAccount.HasVerificationMail(); i++)
				{
					user.EmailAccount.FetchEmails();
					Thread.Sleep(100);
					if (i == 200)
						throw new Exception("Unable to start pro trial! No verification email found.");
				}
				user.VerifyAccount();
				Console.WriteLine("Email: {0}", user.EmailAccount.Address);
				Console.WriteLine("Password: {0}", user.Password);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Dun fucking goofed!");
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey(true);
		}
	}
}