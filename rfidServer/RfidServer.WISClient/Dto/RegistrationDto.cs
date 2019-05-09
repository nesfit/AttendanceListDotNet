using System;
using System.Collections.Generic;
using System.Text;

namespace RfidServer.WisAPI.Dto
{
	public class RegistrationDto
	{
		public int WisCourseId { get; set; }
		public int WisItemId { get; set; }
		public int WisVariantId { get; set; }
		public string CourseAbbrv { get; set; }
		public int Year { get; set; }
		public string Sem { get; set; }
		public int Points { get; set; }
		public string TeacherLogin { get; set; }
		public string TeacherPassword { get; set; }
	}
}
