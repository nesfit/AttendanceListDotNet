using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RfidServer.WisAPI.Dto
{
	public class StudentPostDto
	{
		public int id { get; set; }
		public int person_id { get; set; }
		public string name { get; set; }
		public string email { get; set; }
		public string login { get; set; }
	}
}
