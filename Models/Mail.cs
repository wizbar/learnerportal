using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MimeKit;

// using MimeKit;

namespace learner_portal.Models
{
    public partial class Mail 
    {
        public Mail(string subject, int id, string email, string fromEmail, string cc, string bcc, string body, string createdBy, DateTime dateCreated, string lastUpdateBy, DateTime dateUpdated
             ,List<MailboxAddress> to
             // ,List<MimeEntity> attachments
            )
        {
            Subject = subject;
            Id = id;
            ToEmail = email;
            FromEmail = fromEmail;
            Cc = cc;
            Bcc = bcc;
            Body = body;
            CreatedBy = createdBy;
            DateCreated = dateCreated;
            LastUpdateBy = lastUpdateBy;
            DateUpdated = dateUpdated;
            To = new List<InternetAddress>(to);
            // Attachments = attachments;
        }

        public Mail()
        {
            // To = new List<InternetAddress>(new List<MailboxAddress>());
            // Attachments = new List<MimeEntity>();
        }


        public string Subject { get; set; }
        public long Id { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime DateUpdated { get; set; }
        [NotMapped]
        public List<InternetAddress> To { get; set; }
        // public List<MimeEntity> Attachments { get; set; }
    }
}
