using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RfidServer.WisAPI.Dto;

namespace RfidServer.WisAPI
{
	public partial class WisClient
	{
		private static string rfidUrl = "dns/getid?hex=";

		public static async Task<StudentRfidDto> GetStudentByRfidUid(string uid)
		{
			var url = rfidUrl + uid;
			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var iso = Encoding.GetEncoding("ISO-8859-1");
				var isoBytes = await content.ReadAsByteArrayAsync();
				var str = iso.GetString(isoBytes);

				var student = str.Split(';');
				if (student.Length != 4)
				{
					return null;
				}
				var studentRfidDto = StudentRfidDto.Create(student);
				return studentRfidDto;
			}

			return null;
		}
	}
}
