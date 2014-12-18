using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WolfreeAlpha.Mail
{
    abstract class MailAccountBase : ITemporaryMail
    {
	    public string Address
        {
            get { return address ?? (address = FetchAddress()); }
            protected set { address = value; }
        }


        protected List<IBasicEmail> emails;
        private string address;

        public bool HasRegistrationMail()
        {
	        return emails.Any(e => e.Subject == "Welcome to Wolfram|Alpha");
        }

        public bool HasVerificationMail()
        {
	        return emails.Any(e => e.Subject == "Wolfram Alpha Pro email verification");
        }
		public bool VerifiedSuccesfully()
		{
			return emails.Any(e => e.Subject == "Welcome to your Wolfram|Alpha Pro trial");
		}
        protected abstract string FetchAddress();
	    public abstract List<IBasicEmail> FetchEmails();
	    public List<IBasicEmail> CachedEmails { get { return emails; } }
    }
}
