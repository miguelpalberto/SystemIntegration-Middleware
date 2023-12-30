using SomiodWebService.Models;
using System.Data.Entity.ModelConfiguration;

namespace SomiodWebService.EntityConfigurations
{
	public class DataConfiguration : EntityTypeConfiguration<Data>
	{
		public DataConfiguration()
		{
			_ = ToTable("data", "somiod");
			_ = HasKey(d => d.Id);
			_ = Property(d => d.Id).HasColumnName("id");
			_ = Property(d => d.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
			_ = HasIndex(d => d.Name).IsUnique();
			_ = Property(d => d.CreatedDate).HasColumnName("creation_dt").IsRequired();
			_ = Property(d => d.Parent).HasColumnName("parent").IsRequired();
			_ = Property(d => d.Content).HasColumnName("content").IsRequired().HasMaxLength(3);
		}
	}
}
