using SomiodWebService.Models;
using System.Data.Entity.ModelConfiguration;

namespace SomiodWebService.EntityConfigurations
{
	public class SubscriptionConfiguration : EntityTypeConfiguration<Subscription>
	{
		public SubscriptionConfiguration()
		{
			_ = ToTable("subscriptions", "somiod");
			_ = HasKey(a => a.Id);
			_ = Property(a => a.Id).HasColumnName("id");
			_ = Property(a => a.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
			_ = HasIndex(a => a.Name).IsUnique();
			_ = Property(a => a.CreatedDate).HasColumnName("creation_dt").IsRequired();
			_ = Property(a => a.Parent).HasColumnName("parent").IsRequired();
			_ = Property(s => s.Endpoint).HasColumnName("endpoint").IsRequired();
			_ = Property(s => s.Event).HasColumnName("events").IsRequired();
		}
	}
}
