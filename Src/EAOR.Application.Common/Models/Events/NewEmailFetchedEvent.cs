using MediatR;

namespace EAOR.Application.Common.Models.Events
{
    public class NewEmailFetchedEvent : INotification
    {
        public string Body { get; set; }
        public byte[] RawData { get; set; }
        public DateTime ReceivedDate { get; set; }
    }
}
