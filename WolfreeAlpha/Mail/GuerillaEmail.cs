using Newtonsoft.Json;

namespace WolfreeAlpha.Mail
{
	/// <summary>
	///     A simple mapper-class, to map GuerillaMail JSON using JSON.NET to a BasicEmail type.
	/// </summary>
	internal class GuerillaEmail : IBasicEmail
	{
		[JsonProperty(PropertyName = "mail_id")]
		public string ID { get; private set; }

		[JsonProperty(PropertyName = "mail_subject")]
		public string Subject { get; private set; }

		public string Body { get; set; }

		[JsonProperty(PropertyName = "mail_from")]
		public string FromAddress { get; private set; }
	}
}