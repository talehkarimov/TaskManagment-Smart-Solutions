namespace TaskManagment.Services.Abstract
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string html);
    }
}
