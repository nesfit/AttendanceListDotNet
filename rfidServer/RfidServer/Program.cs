using System;
using System.Security.Permissions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RfidServer.DAL;

namespace RfidServer
{
	public class Program
	{
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		public static void Main(string[] args)
		{
			var webHost = CreateWebHostBuilder(args).Build();

			using (var scope = webHost.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var db = services.GetRequiredService<RegistrationDbContext>();
					db.Database.Migrate();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while migrating the database.");
				}
			}

			webHost.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			//var configuration = new ConfigurationBuilder()
			//	.AddCommandLine(args)
			//	.Build();

			//var hostUrl = configuration["hosturl"];
			//if (string.IsNullOrEmpty(hostUrl))
			//	hostUrl = "http://0.0.0.0:6000";

			return WebHost.CreateDefaultBuilder(args)
				//.UseUrls(hostUrl)
				.UseStartup<Startup>();
		}
	}
}
