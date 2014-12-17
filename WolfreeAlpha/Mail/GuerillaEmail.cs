namespace WolfreeAlpha.Mail
{
    internal class GuerillaEmail : IBasicEmail
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string FromAddress { get; private set; }
    }
}