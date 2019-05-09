using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RfidServer.WisAPI;

namespace RfidServer.Web.Controllers
{
	public class BaseController : Controller
	{
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			ViewBag.isLogged = await IsUserLogged();
			if (ViewBag.isLogged)
			{
				ViewBag.Username = HttpContext.Session.GetString("_Username");
			}
			else
			{
				context.Result = RedirectToAction("Index", "Login");
			}
			await base.OnActionExecutionAsync(context, next);
		}

		public async Task<bool> IsUserLogged()
		{
			var username = HttpContext.Session.GetString("_Username");
			var auth = HttpContext.Session.GetString("_Auth");
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(auth))
			{
				return false;
			}
			return await WisClient.IsUserLogged(username, auth);
		}
	}
}
