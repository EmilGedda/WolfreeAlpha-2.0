using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace WolfreeAlpha.Mail
{
    internal interface ITemporaryMail
    {
        string Address { get; }
        bool HasRegistrationMail();
        bool HasVerificationMail();
	    List<IBasicEmail> FetchEmails();
	    List<IBasicEmail> CachedEmails { get; }
    }
}