using SomiodWebService.Migrations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;

namespace SomiodWebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SomiodDbContext>());

			using (var context = new SomiodDbContext())
			{
				if (!context.Database.Exists())
				{
					// Create the database if it does not exist
					context.Database.Create();

					// Apply migrations
					var configuration = new Configuration();
					var migrator = new DbMigrator(configuration);
					migrator.Update();
				}
				context.Database.Initialize(false);
			}
		}
    }
}
