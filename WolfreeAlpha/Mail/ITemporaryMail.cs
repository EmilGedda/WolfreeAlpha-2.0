using System.Collections.Generic;

namespace WolfreeAlpha.Mail
{
	internal interface ITemporaryMail
	{
		string Address { get; }
		List<IBasicEmail> CachedEmails { get; }
		bool HasRegistrationMail();
		bool HasVerificationMail();
		List<IBasicEmail> FetchEmails();
	}
}