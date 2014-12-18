namespace WolfreeAlpha.Mail
{
    internal interface IBasicEmail
    {
        string Subject { get; }
        string Body { get; }
        string FromAddress { get; }
    }
}