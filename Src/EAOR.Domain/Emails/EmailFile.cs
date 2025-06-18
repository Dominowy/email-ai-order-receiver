using EAOR.Domain.Common;

namespace EAOR.Domain.Emails
{
    public class EmailFile : Entity
    {
        public byte[] Data { get; private set; }

        public Guid EmailId { get; private set; }
        public virtual Email Email{ get; private set; }

        protected EmailFile() : base()
        {

        }

        public EmailFile(byte[] data, Guid emailId) : base(Guid.NewGuid())
        {
            Data = data;
            EmailId = emailId;
        }
    }
}
