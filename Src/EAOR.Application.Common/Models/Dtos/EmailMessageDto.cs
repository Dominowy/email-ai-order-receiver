namespace EAOR.Application.Common.Models.Dtos
{
    public class EmailMessageDto
    {
        public string Body { get; set; }
        public DateTime ReceivedDate { get; set; }
        public byte [] Data { get; set; }
    }
}