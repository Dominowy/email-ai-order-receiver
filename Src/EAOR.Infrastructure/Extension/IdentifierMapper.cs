using EAOR.Application.Common.Models.Identifier;
using MailKit;

namespace EAOR.Infrastructure.Extension
{
	public static class IdentifierMapper
	{
		public static Identifier ToEmailIdentifier(this UniqueId uniqueId)
			=> new Identifier { Id = uniqueId.Id };

		public static UniqueId ToUniqueId(this Identifier identifier)
			=> new UniqueId(identifier.Id);
	}
}
