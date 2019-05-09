using System.Collections.Generic;
using RfidServer.DAL.Entity.Base;

namespace RfidServer.DAL.Entity
{
	public class Variant : EntityBase
	{
		public int WisId { get; set; }
		public int WisItemId { get; set; }
		public int WisCourseId { get; set; }
		public string Title { get; set; }
		public int Points { get; set; }
		public int Limit { get; set; }
		public string CourseAbbrv { get; set; }
		public int Year { get; set; }
		public string Sem { get; set; }
		public virtual ICollection<Student> Students { get; set; } = new List<Student>();
	}
}
