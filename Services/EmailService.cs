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

        public async Task Execute(string subject, string to, string plainTextContent, string html)
        {
            var apiKey = Data.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Data.Gmail);
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent, html);
            var response = await client.SendEmailAsync(msg);
        }
        //public async Task Execute(string subject, string to, string plainTextContent, string html)
        //{
        //    var emailMessage = new MimeMessage();

        //    emailMessage.From.Add(new MailboxAddress("Администрация сайта", "max.shuldiner777@gmail.com"));
        //    emailMessage.To.Add(new MailboxAddress("", "shuldiner@gmail.com"));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = plainTextContent
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync("smtp.sendgrid.net", 25, false);
        //        await client.AuthenticateAsync("apikey", "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");
        //        await client.SendAsync(emailMessage);
        //        await client.DisconnectAsync(true);
        //    }
        //}


    }
}