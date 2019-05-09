using System.ComponentModel.DataAnnotations.Schema;

namespace RfidServer.DAL.Entity.Base
{
	public abstract class EntityBase
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
	}
}
