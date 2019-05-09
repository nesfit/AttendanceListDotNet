using System.Threading.Tasks;
using Newtonsoft.Json;
using RfidServer.WisAPI.Dto;

namespace RfidServer.WisAPI
{
	public partial class WisClient
	{
		private static string restUrl = "db/vyuka/rest/";

		public static async Task<CourseDto[]> GetCoursesAsync(string abbrv)
		{
			CourseDto[] coursesDto;
			abbrv = abbrv.ToUpper();
			var url = restUrl + "courses";
			if (abbrv != "") {
				url += "?abbrv=" + abbrv;
			}
	
			var response = Client.SendAsync(GetRequest(url)).Result;
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<CourseDto>>(content);
				coursesDto = new[] { obj?.data };
			}
			else
				coursesDto = null;

			return coursesDto;
		}

		public static async Task<CourseDto> GetCourseByIdAsync(int courseId)
		{
			CourseDto courseDto;
			var url = restUrl + "course/" + courseId;

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<CourseDto>>(content);
				courseDto = obj?.data;
			}
			else
				courseDto = null;

			return courseDto;
		}

		public static async Task<ItemDto[]> GetItemsAsync(int courseId)
		{
			ItemDto[] itemsDto;
			var url = restUrl + "course/" + courseId + "/items";

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<ItemDto[]>>(content);
				itemsDto = obj?.data;
			}
			else
				itemsDto = null;

			return itemsDto;
		}

		public static async Task<ItemDto> GetItemByIdAsync(int courseId, int itemId)
		{
			ItemDto itemDto;
			var url = restUrl + "course/" + courseId + "/item/" + itemId;

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<ItemDto>>(content);
				itemDto = obj?.data;
			}
			else
				itemDto = null;

			return itemDto;
		}

		public static async Task<StudentDto[]> GetStudentsAsync(int courseId, int itemId)
		{
			StudentDto[] studentsDto;
			string url = restUrl + "course/" + courseId + "/item/" + itemId + "/students";

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<StudentDto[]>>(content);
				studentsDto = obj?.data;
			}
			else
				studentsDto = null;

			return studentsDto;
		}
		
		public static async Task<StudentCourseDto[]> GetCourseStudentsAsync(int courseId)
		{
			StudentCourseDto[] dto;
			string url = restUrl + "course/" + courseId + "/students";

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<StudentCourseDto[]>>(content);
				dto = obj?.data;
			}
			else
				dto = null;

			return dto;
		}

		public static async Task<VariantDto[]> GetVariantsAsync(int courseId, int itemId)
		{
			VariantDto[] variantsDto;
			var url = restUrl + "course/" + courseId + "/item/" + itemId + "/variants";

			var response = await Client.SendAsync(GetRequest(url));
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var obj = await ContentToObj<WisResponse<VariantDto[]>>(content);
				variantsDto = obj?.data;
			}
			else
				variantsDto = null;

			return variantsDto;
		}
	}
}
