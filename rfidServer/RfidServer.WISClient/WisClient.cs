using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RfidServer.WisAPI.Dto;

namespace RfidServer.WisAPI
{
	public partial class WisClient
	{
		private static string BaseUrl = "https://wis.fit.vutbr.cz/FIT/";
		public static string Username;
		public static string AuthBase64;
		public static bool AutoRegister;
		public static int? ActiveCourseId;
		private static StudentCourseDto[] _Cached;
		private static readonly HttpClient Client = new HttpClient();

		static WisClient()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			Client.DefaultRequestHeaders.Accept.Clear();
			Client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
		}

		private static async Task<T> ContentToObj<T>(HttpContent content)
		{
			var utf8 = Encoding.UTF8;
			var iso = Encoding.GetEncoding("ISO-8859-1");
			var utfBytes = await content.ReadAsByteArrayAsync();
			var str = Encoding.UTF8.GetString(utfBytes);
			str = Regex.Unescape(str);
			T obj;
			try
			{
				obj = JsonConvert.DeserializeObject<T>(str);
			}
			catch (JsonSerializationException)
			{
				obj = default(T);
			}
			return obj;
		}

		public static string GenerateBase64(string username, string password)
		{
			var byteArray = new UTF8Encoding().GetBytes($"{username}:{password}");
			return Convert.ToBase64String(byteArray);
		}

		public static string[] FromBase64(string authBase64)
		{
			var byteCredentials = Convert.FromBase64String(authBase64);
			return Encoding.UTF8.GetString(byteCredentials).Split(":");
		}

		public static HttpRequestMessage GetRequest(string url)
		{
			var requestUrl = new Uri(BaseUrl + url);
			var request = new HttpRequestMessage()
			{
				RequestUri = requestUrl,
				Method = HttpMethod.Get
			};
			return request;
		}

		public static HttpRequestMessage PostRequest(string url, string content)
		{
			var requestUrl = new Uri(BaseUrl + url);
			var request = new HttpRequestMessage()
			{
				RequestUri = requestUrl,
				Method = HttpMethod.Post
			};
			var buffer = System.Text.Encoding.UTF8.GetBytes(content);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = byteContent;
			return request;
		}

		public static async Task<StudentCourseDto[]> GetStudents(int courseId)
		{
			if (ActiveCourseId != courseId || _Cached == null)
			{
				_Cached = await GetCourseStudentsAsync(courseId);
			}
			return _Cached;
		}
		
		public static async Task<bool> Login(string username, string authBase64)
		{
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
			if (await IsLogged())
			{
				Username = username;
				AuthBase64 = authBase64;
				return true;
			}
			Client.DefaultRequestHeaders.Authorization = null;
			return false;
		}

		public static async Task<bool> IsUserLogged(string username, string authBase64)
		{
			return username == Username
			       && authBase64 == AuthBase64
			       && await IsLogged();
		}

		public static async Task<bool> IsLogged()
		{
			try
			{
				HttpResponseMessage response = await Client.SendAsync(GetRequest("db/vyuka/rest/courses"));
				return response.IsSuccessStatusCode;
			}
			catch (HttpRequestException)
			{
				return false;
			}
		}

		public static void Logout()
		{
			Username = null;
			AuthBase64 = null;
			Client.DefaultRequestHeaders.Authorization = null;
		}
	}
}
