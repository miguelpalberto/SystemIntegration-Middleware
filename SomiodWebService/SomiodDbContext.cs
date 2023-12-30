using SomiodWebService.Models;
using System.Data.Entity;

namespace SomiodWebService
{
	public class SomiodDbContext : DbContext
	{
		public SomiodDbContext() : base("Somiod")
		{
		}

		public DbSet<Application> Applications { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			_ = modelBuilder.HasDefaultSchema("somiod");
			_ = modelBuilder.Configurations.Add(new EntityConfigurations.ApplicationConfiguration());
			base.OnModelCreating(modelBuilder);
		}
	}
}
