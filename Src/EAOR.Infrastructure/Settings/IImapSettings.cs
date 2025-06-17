using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAOR.Infrastructure.Settings
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
