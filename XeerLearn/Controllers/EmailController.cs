using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using System.Net.Mail;
using System.Configuration;
using RazorHtml.RazorClassLib.Services;
using System.Net;

namespace XeerLearn.Controllers
{
    public class EmailController : Controller
    {
        public IConfiguration _configuration { get; set; }
        public readonly IRazorViewToStringRenderer _render;

        public EmailController(IRazorViewToStringRenderer render, IConfiguration configuration)
        {
            _render = render;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(dynamic model)
        {

            var sender = _configuration.GetConnectionString("Sender");
            var sender_name = _configuration.GetConnectionString("Name");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender_name, sender));

            message.To.Add(new MailboxAddress($"{model.FirstName} {model.LastName}", model.Email));

            message.Subject = model.Subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = model.Body
            };

            var username = _configuration.GetConnectionString("Username");
            var password = _configuration.GetConnectionString("Password");

            using var client = new SmtpClient
            {
                Credentials = new NetworkCredential(username, password),
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Port = 587,
                Host = "smtp.gmail.com",
                
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(message.From[0].ToString());
            mail.To.Add(new MailAddress(message.To[0].ToString()));
            mail.Subject = message.Subject;
            mail.IsBodyHtml = true;
            mail.Body = message.HtmlBody;
                 

            await client.SendMailAsync(mail);
        }

    }
}
