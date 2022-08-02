using System.Net;
using System.Net.Mail;

namespace WebApplication_MyNoteSampleApp.Core
{
    public class MailHelper
    {
        public void SendMail(string subject,string body, params string[] tos)
        {
            SmtpClient client = new SmtpClient("smtp.mail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("goksu-bunyamin@hotmail.com", "123456")
            };

            MailMessage mailMessage = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };
            mailMessage.From = new MailAddress("goksu-bunyamin@hotmail.com");

            foreach (string eMailAddress in tos)
            {
                mailMessage.To.Add(eMailAddress);
            }

            client.Send(mailMessage);
        }
    }
}
