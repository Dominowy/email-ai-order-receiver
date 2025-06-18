﻿using EAOR.Application.Contracts.Configuration;

namespace EAOR.Infrastructure.Settings
{
	public class ImapSettings : IImapSettings
	{
		public string Host { get; set; } = string.Empty;
		public int Port { get; set; }
		public bool UseSsl { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
