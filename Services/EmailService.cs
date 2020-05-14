using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace AmitTextile.Services
{
    public class EmailService 
    {
       
        
        private EmailAuthData Data { get; } = new EmailAuthData();

        public EmailService(IConfiguration configuration)
        {
            configuration.GetSection(nameof(EmailAuthData)).Bind(Data);
            
        }

        //public async Task Execute(string subject, string to, string plainTextContent, string html)
        //{
        //    var apiKey = Data.ApiKey;
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress(Data.Gmail);
        //    var toEmail = new EmailAddress(to);
        //    var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent, html);
        //    var response = await client.SendEmailAsync(msg);
        //    int x = 5;
        //}
        public async Task Execute(string subject, string to, string plainTextContent, string html)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Data.UserNameForSmtp));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = html
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(Data.UserNameForSmtp, Data.PasswordForSmtp);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }


    }
}