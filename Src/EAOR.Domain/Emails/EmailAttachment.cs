using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAOR.Domain.Emails
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public Guid EmailMessageId { get; set; }
        public virtual EmailMessage EmailMessage { get; set; }

        protected EmailAttachment() : base()
        {

        }

        public EmailAttachment(string fileName, string contentType, byte[] data, Guid emailMessageId) : base()
        {
            FileName = fileName;
            ContentType = contentType;
            Data = data;
            EmailMessageId = emailMessageId;
        }
    }
}
