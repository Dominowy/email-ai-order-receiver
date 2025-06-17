using EAOR.Domain.Common;

namespace EAOR.Domain.Emails
{
    public class EmailFile : Entity
    {
        public string FileName { get; private set; }

        public byte[] Data { get; private set; }

        public Guid EmailId { get; private set; }
        public virtual Email Email{ get; private set; }

        protected EmailFile() : base()
        {

        }

        public EmailFile(string fileName, string contentType, byte[] data, Guid emailId) : base(Guid.NewGuid())
        {
            FileName = fileName;
            Data = data;
            EmailId = emailId;
        }
    }
}
