using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RfidServer.WisAPI.Dto;

namespace RfidServer.WisAPI
{
	public partial class WisClient
	{
		public static async Task<List<int>> RegisterStudents(List<StudentPostDto> students, RegistrationDto registration)
		{
			var studentIds = new List<int>();
			var credentials = FromBase64(AuthBase64);
			if (registration == null || !students.Any() || credentials.Length != 2)
			{
				return studentIds;
			}

			var url = restUrl + "course/" + registration.WisCourseId + "/item/" + registration.WisVariantId + "/students";
			foreach (var student in students)
			{
				var content = JsonConvert.SerializeObject(student);
				//content = Regex.Replace(content, @"[^\x00-\x7F]", c =>
				//	string.Format(@"\u{0:x4}", (int)c.Value[0]));
				var postRequest = PostRequest(url, content);
				var response = await Client.SendAsync(postRequest);
				if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.UnprocessableEntity)
				{
					registration.TeacherLogin = credentials[0];
					registration.TeacherPassword = credentials[1];
					var evalSent = SendEvaluation(student, registration);
					if (evalSent)
					{
						studentIds.Add(student.id);
					}
				}
			}

			return studentIds;
		}
	}
}
