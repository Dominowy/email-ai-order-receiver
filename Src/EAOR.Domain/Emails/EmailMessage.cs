using EAOR.Domain.Common;

namespace EAOR.Domain.Emails
{
    public class EmailMessage : BaseEntity
    {
        public string Subject { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public string PlainTextBody { get; set; }
        public string HtmlBody { get; set; }

        public byte[] RawEml { get; set; }

        public virtual ICollection<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();

        protected EmailMessage() : base()
        {
        }

        protected EmailMessage(Guid id) : base(id)
        {
        }
    }
}
