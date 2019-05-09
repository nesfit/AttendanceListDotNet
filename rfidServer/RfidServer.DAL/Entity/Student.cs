using System;
using RfidServer.DAL.Entity.Base;

namespace RfidServer.DAL.Entity
{
	public class Student : EntityBase
	{
		public int WisId { get; set; }
		public int WisPersonId { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public int Points { get; set; } = 0;
		public string Who { get; set; } = "";
		public string RegType { get; set; } = "zam";
		public string Date { get; set; }
		public string RegTime { get; set; }
		public string Update { get; set; }
		public bool Registered { get; set; } = false;
		public int VariantId { get; set; }
		public virtual Variant RegisteredVariant { get; set; }
	}
}
