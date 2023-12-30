using System.Data.Entity.Migrations;

namespace SomiodWebService.Migrations
{
	public partial class SubscriptionTable : DbMigration
	{
		public override void Up()
		{
			_ = CreateTable(
				"somiod.subscriptions",
				c => new
				{
					id = c.Int(nullable: false, identity: true),
					events = c.String(nullable: false),
					endpoint = c.String(nullable: false),
					parent = c.Int(nullable: false),
					name = c.String(nullable: false, maxLength: 255),
					creation_dt = c.String(nullable: false),
				})
				.PrimaryKey(t => t.id)
				.ForeignKey("somiod.containers", t => t.parent, cascadeDelete: true)
				.Index(t => t.parent)
				.Index(t => t.name, unique: true);

		}

		public override void Down()
		{
			DropForeignKey("somiod.subscriptions", "parent", "somiod.containers");
			DropIndex("somiod.subscriptions", new[] { "name" });
			DropIndex("somiod.subscriptions", new[] { "parent" });
			DropTable("somiod.subscriptions");
		}
	}
}
