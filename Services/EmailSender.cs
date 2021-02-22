
using System;
using learner_portal.Helpers;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit; 
using learner_portal.Models;

namespace learner_portal.Services
{
    public class EmailSender : IEmailSender, IDisposable
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly LearnerContext _context;
        private readonly ILogger<EmailSender> _logger;
        private bool _disposed;

        public EmailSender(EmailConfiguration emailConfig, LearnerContext context,ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig;
            _context = context;
            _logger = logger;
        }

                
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this._disposed = true;
            }
        }
        public void SendEmail(Mail message)
        {
            var emailMessage = CreateEmailMessage(message);
         //   _logger.LogDebug("Add a copy of the email in the DB...");
            Send(emailMessage);
            _context.Mail.Add(message);
             _context.SaveChangesAsync();
           //  _context.Database.CloseConnectionAsync();
        }      
        
        
        private MimeMessage CreateEmailMessage(Mail message)
        {
         //   _logger.LogDebug("Prepare ...");
            
            //Create an Email Object
            var emailMessage = new MimeMessage();
            //Create a body builder to add attachments
            var builder = new BodyBuilder();   
            //Created an Email 
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange( message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };
            
        
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
           // _logger.LogInformation(" sending an email to : " + mailMessage);
            using (var client = new SmtpClient())
            {
                try 
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
        
                    client.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    
                    _logger.LogInformation("An error has occured while sending an email..." + ex.StackTrace);
                   
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                 //   client.Dispose();
                }
            }
        }
        
    }
}