namespace EAOR.Application.Common.Models.Message
{
    public class EmailMessage
    {
        public string Body { get; set; }
        public DateTime ReceivedDate { get; set; }
        public byte [] Data { get; set; }
    }
}