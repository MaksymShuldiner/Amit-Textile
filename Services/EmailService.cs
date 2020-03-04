using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AmitTextile.Services
{
    public class EmailService 
    {
       
        
        private EmailAuthData Data { get; set; } = new EmailAuthData();

        public EmailService(IConfiguration configuration)
        {
            configuration.GetSection(nameof(EmailAuthData)).Bind(Data);
            
        }

        public async Task Execute(string subject, string to, string plainTextContent)
        {
            var apiKey = Data.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Data.Gmail);
            var toEmail = new EmailAddress(to);
            var htmlcontent = "";
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent,htmlcontent);
            var response = await client.SendEmailAsync(msg);
           
        }



    }
}