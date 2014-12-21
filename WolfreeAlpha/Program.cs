using System;
using System.Diagnostics;
using System.Threading;
using WolfreeAlpha.Mail;

namespace WolfreeAlpha
{
	internal static class Program
	{
		private static Stopwatch sw;

		private static readonly Action<User>[] flow =
		{
			u => u.EmailAccount = new GuerillaMailAccount(u),
			u => u.CreateAccount(),
			u => WaitForMail(user => user.EmailAccount.HasRegistrationMail(), u,
					"Unable to create account! No registration email found."),
			u => u.StartProTrial(),
			u => WaitForMail(user => user.EmailAccount.HasVerificationMail(), u,
					"Unable to start pro trial! No verification email found."),
			u => u.VerifyAccount()
		};

		private static readonly string[] messages =
		{
			"Creating temporary email", "Creating wolfram alpha account", "Waiting for registration mail...",
			"Starting pro trial", "Waiting for verification mail...", "Verifying account"
		};

		private static void Main(string[] args)
		{
			try
			{
				sw = Stopwatch.StartNew();
				PrintLine("Generating name");
				User user = User.CreateRandomUser();
				for (int i = 0; i < flow.Length; i++)
				{
					PrintLine(messages[i]);
					flow[i].Invoke(user);
				}
				PrintLine("Done");
				sw.Stop();
				Console.WriteLine();
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

		private static void PrintLine(string message)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("[{0:00.000}] ", sw.Elapsed.TotalSeconds);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(message);
		}

		private static void WaitForMail(Predicate<User> pred, User user, string exceptionMsg)
		{
			user.EmailAccount.FetchEmails();
			for (int i = 0; !pred.Invoke(user); i++)
			{
				Thread.Sleep(500);
				if (i == 20) throw new Exception(exceptionMsg);
				user.EmailAccount.FetchEmails();
			}
		}
	}
}