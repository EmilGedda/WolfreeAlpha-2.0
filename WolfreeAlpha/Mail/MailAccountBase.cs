using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }

        public bool HasVerificationMail()
        {
            throw new NotImplementedException();
        }

        protected abstract string FetchAddress();
    }
}
