﻿using SomiodWebService.Models;
using System.Data.Entity;

namespace SomiodWebService
{
	public class SomiodDbContext : DbContext
	{
		public SomiodDbContext() : base("Somiod")
		{
		}

		public DbSet<Application> Applications { get; set; }
		public DbSet<Container> Containers { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<Data> Data { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			_ = modelBuilder.HasDefaultSchema("somiod");
			_ = modelBuilder.Configurations.Add(new EntityConfigurations.ApplicationConfiguration());
			_ = modelBuilder.Configurations.Add(new EntityConfigurations.ContainerConfiguration());
			_ = modelBuilder.Configurations.Add(new EntityConfigurations.SubscriptionConfiguration());
			_ = modelBuilder.Configurations.Add(new EntityConfigurations.DataConfiguration());
			base.OnModelCreating(modelBuilder);
		}
	}
}
