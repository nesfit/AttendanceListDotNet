using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RfidServer.WisAPI;

namespace RfidServer.Web.Controllers
{
    public class LoginController : Controller
	{
	    public IActionResult Index()
	    {
		    return View();
	    }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(string username, string password)
		{
			string authBase64 = WisClient.GenerateBase64(username, password);
			bool isLoginCorrect = await WisClient.Login(username, authBase64);
			if (isLoginCorrect)
			{
				HttpContext.Session.Clear();
				HttpContext.Session.SetString("_Username", username);
				HttpContext.Session.SetString("_Auth", authBase64);
				return RedirectToAction("Index", "Students");
			}
			ViewBag.LoginFailed = true;
			return View();
		}

		public IActionResult Logout()
		{
			WisClient.Logout();
			HttpContext.Session.Clear();
			ViewBag.isLogged = false;
			return RedirectToAction("Index", "Students");
		}

		//public static bool RecycleApplicationPool(string siteName = null)
		//{
		//	if (siteName == null) siteName = HostingEnvironment.ApplicationHost.GetSiteName();
		//	using (ServerManager iisManager = new ServerManager())
		//	{
		//		SiteCollection sites = iisManager.Sites;
		//		foreach (Site site in sites)
		//		{
		//			if (site.Name == siteName)
		//			{
		//				iisManager.ApplicationPools[site.Applications["/"].ApplicationPoolName].Recycle();
		//				return true;
		//			}
		//		}
		//	}
		//	return false;
		//}
	}
}