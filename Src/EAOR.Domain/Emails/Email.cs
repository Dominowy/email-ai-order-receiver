using EAOR.Domain.Common;

namespace EAOR.Domain.Emails
{
    public class Email : Entity
    {
        public string Body { get; private set; }
        public DateTime ReceivedDate { get; private set; }

        public virtual EmailFile EmailFile { get; private set; }

        protected Email() : base()
        {
        }

        public Email(string body, DateTime receivedDate) : base(Guid.NewGuid())
        {
            Body = body;
            ReceivedDate = receivedDate;
        }
    }
}
