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
            
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent,html);
            var response = await client.SendEmailAsync(msg);
           
        }



    }
}