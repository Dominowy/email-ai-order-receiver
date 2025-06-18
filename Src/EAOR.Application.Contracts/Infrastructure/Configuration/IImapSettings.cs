namespace EAOR.Application.Contracts.Configuration
{
	public interface IImapSettings
	{
		string Host { get; }
		int Port { get; }
		bool UseSsl { get; }
		string Username { get; }
		string Password { get; }
	}
}
