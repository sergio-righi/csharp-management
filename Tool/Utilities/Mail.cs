using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Tool.Utilities
{
    public static class Mail
    {
        public static async Task Send(string subject, string content, string[] recipient)
        {
            SmtpClient smtp = new SmtpClient("")
            {
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true,
                Port = 587
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(""),
                IsBodyHtml = true,
                Subject = subject,
                Body = content
            };

            for (var i = 0; i < recipient.Length; i++)
            {
                mail.To.Add(recipient[i]);
            }

            //smtp.Send(mail);
        }
    }
}
