using System;
using System.Drawing;
using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            SendGridSecret = _config["SendGrid:SecretKey"];
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("minhson0506@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            ////send email with GMAIL ACCOUNT
            using (var emailClient = new System.Net.Mail.SmtpClient())
            {
                //emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //emailClient.Authenticate("richfry6e");
                //emailClient.Send(emailToSend);
                //emailClient.Disconnect(true);


                //emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //emailClient.EnableSsl = true;
                //emailClient.Host = "smtp.gmail.com";
                //emailClient.Port = 587;
                //// setup Smtp authentication
                //System.Net.NetworkCredential credentials =
                //    new System.Net.NetworkCredential("minhson0506@gmail.com", "MinhSonKhanhChi@123");
                //emailClient.UseDefaultCredentials = false;
                //emailClient.Credentials = credentials;

                //MailMessage msg = new MailMessage();
                //msg.From = new MailAddress("minhson0506@gmail.com");
                //msg.To.Add(new MailAddress(email));

                //msg.Subject = "This is a test Email subject";
                //msg.IsBodyHtml = true;
                //msg.Body = string.Format("<html><head></head><body><b>Test HTML Email</b></body>");
                //emailClient.Send(msg);
                //emailClient.Disconnect(true);

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                // setup Smtp authentication
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential("minhson0506@gmail.com", "MinhSonKhanhChi@123");
                
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("minhson0506@gmail.com");
                msg.To.Add(new MailAddress(email));

                msg.Subject = "This is a test Email subject";
                msg.IsBodyHtml = true;
                msg.Body = string.Format("<html><head></head><body><b>Test HTML Email</b></body>");

                try
                {
                    client.Send(msg);
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("fail to send mess" + ex.Message);
                };
            }

            return Task.CompletedTask;

            //var client = new SendGridClient(SendGridSecret);
            ////THIS MUST MATCH A VERIFIED SENDGRID E-MAIL ADDRESS!!
            //var from = new EmailAddress("minhson0506@gmail.com", "TradeHub");
            //var to = new EmailAddress(email);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            //return client.SendEmailAsync(msg);

        }
    }
}

