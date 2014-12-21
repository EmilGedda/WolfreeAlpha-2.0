using System.Collections.Generic;
using System.Linq;

namespace WolfreeAlpha.Mail
{
	internal abstract class MailAccountBase : ITemporaryMail
	{
		private string address;
		protected List<IBasicEmail> emails;

		public string Address
		{
			get { return address ?? (address = FetchAddress()); }
			protected set { address = value; }
		}

		public bool HasRegistrationMail()
		{
			return emails.Any(e => e.Subject == "Welcome to Wolfram|Alpha");
		}

		public bool HasVerificationMail()
		{
			return emails.Any(e => e.Subject == "Wolfram Alpha Pro email verification");
		}

		public abstract List<IBasicEmail> FetchEmails();

		public List<IBasicEmail> CachedEmails
		{
			get { return emails; }
		}

		public bool VerifiedSuccesfully()
		{
			return emails.Any(e => e.Subject == "Welcome to your Wolfram|Alpha Pro trial");
		}

		protected abstract string FetchAddress();
	}
}