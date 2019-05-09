using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using CookComputing.XmlRpc;
using RfidServer.WisAPI.Dto;

namespace RfidServer.WisAPI
{
	[XmlRpcUrl("https://wis.fit.vutbr.cz/FIT/db/vyuka/ucitel/course-item-xml.php")]
	class TestProxy : XmlRpcClientProtocol
	{
		[XmlRpcMethod("courses.set_item_points")]
		public string GetStateName(string abbrv, int acy, string sem, int item,
			string stud_login, double points, string comment, string login,
			DateTime date, int doupdate)
		{
			return (string) Invoke(MethodBase.GetCurrentMethod(), new object[]
				{abbrv, acy, sem, item, stud_login, points, comment, login, date, doupdate});
		}
	}

	public partial class WisClient
	{
		public static bool SendEvaluation(StudentPostDto studentDto, RegistrationDto registrationDto)
		{

			var proxy = new TestProxy
			{
				Credentials = new NetworkCredential(registrationDto.TeacherLogin, registrationDto.TeacherPassword),
				UseIntTag = true
			};

			try
			{
				var result = proxy.GetStateName(registrationDto.CourseAbbrv, registrationDto.Year,
					registrationDto.Sem, registrationDto.WisVariantId,studentDto.login,
					25, "", registrationDto.TeacherLogin,
					DateTime.Today, 1);
				if (result != "OK Change" && result != "NO Change")
					return false;

				return true;
			}
			catch (XmlRpcFaultException ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return false;
		}
	}
}