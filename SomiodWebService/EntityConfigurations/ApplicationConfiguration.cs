using SomiodWebService.Models;
using System.Data.Entity.ModelConfiguration;

namespace SomiodWebService.EntityConfigurations
{
	public class ApplicationConfiguration : EntityTypeConfiguration<Application>
	{
		public ApplicationConfiguration()
		{
			_ = ToTable("applications", "somiod");
			_ = HasKey(a => a.Id);
			_ = Property(a => a.Id).HasColumnName("id");
			_ = Property(a => a.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
			_ = HasIndex(a => a.Name).IsUnique();
			_ = Property(a => a.CreatedDate).HasColumnName("creation_dt").IsRequired();
		}
	}
}
