using System.Data.Entity.Migrations;

namespace SomiodWebService.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<SomiodWebService.SomiodDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(SomiodWebService.SomiodDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method
			//  to avoid creating duplicate seed data.
		}
	}
}
