using System.Data.Entity.Migrations;

namespace SomiodWebService.Migrations
{
	public partial class InitialMigration : DbMigration
	{
		public override void Up()
		{
			_ = CreateTable(
				"somiod.applications",
				c => new
				{
					id = c.Int(nullable: false, identity: true),
					name = c.String(nullable: false, maxLength: 255),
					creation_dt = c.String(nullable: false),
				})
				.PrimaryKey(t => t.id);

		}

		public override void Down()
		{
			DropTable("somiod.applications");
		}
	}
}
