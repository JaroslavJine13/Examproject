using System.Net.Mail;
using System.Security.Claims;
using WebMud.Models;

namespace WebMud.Data
{
    public class EmailSender: IEmailServices
    {
        public async Task<bool> SendEmailAsync(string emailDestination, string subject, string htmlMessageBody)
        {

            MailMessage ms = new MailMessage(AdminSettings.EmailDisplayed, emailDestination, subject, htmlMessageBody);
            ms.IsBodyHtml = true;
            string user = AdminSettings.EmailGeneric;
            string passcode = AdminSettings.EmailPassword;
            SmtpClient smtpServer = new SmtpClient(AdminSettings.Smtp, AdminSettings.Port);
            smtpServer.Credentials = new System.Net.NetworkCredential(user, passcode);
            smtpServer.EnableSsl= AdminSettings.IsSsl;
            try
            {
                await smtpServer.SendMailAsync(ms);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            return true;
        }



    }
}
