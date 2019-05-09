using System;
using System.Collections.Generic;
using System.Text;

namespace RfidServer.WisAPI.Dto
{
	public class StudentRfidDto
	{
		public string Login { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
		public string Faculty { get; set; }

		public static StudentRfidDto Create(string[] data)
		{
			return new StudentRfidDto
			{
				Login = data[0],
				Name = data[1],
				Role = data[2],
				Faculty = data[3]
			};
		}
	}
}
