using EAOR.Domain.Common;

namespace EAOR.Domain.Emails
{
    public class Email : Entity
    {
        public string Body { get; set; }
        public DateTime ReceivedDate { get; set; }

        public virtual EmailFile EmailFile { get; set; }

        protected Email() : base()
        {
        }

        protected Email(string body, DateTime receivedDate) : base(Guid.NewGuid())
        {
            Body = body;
            ReceivedDate = receivedDate;
        }
    }
}
