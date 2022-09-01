using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using TaskManagment.Services.Abstract;

namespace TaskManagment.Services.Concrete
{
    public class EmailService: IEmailService
    {
        public void SendEmail(string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("test@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate("test@gmail.com", "test");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
