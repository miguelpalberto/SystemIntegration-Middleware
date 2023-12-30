using System.Data.Entity;
using System.Web.Http;

namespace SomiodWebService
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SomiodDbContext>());
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<SomiodDbContext, Migrations.Configuration>());

			using (var context = new SomiodDbContext())
			{
				context.Database.Initialize(false);
			}
		}
	}
}
