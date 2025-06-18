namespace EAOR.Infrastructure.Services.Response
{
	public class LlmResponse
	{
		public Choice[] choices { get; set; }
	}

	public class Choice
	{
		public Message message { get; set; }
	}

	public class Message
	{
		public string content { get; set; }
	}
}
