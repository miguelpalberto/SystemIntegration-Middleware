using System.Data.Entity.Migrations;

namespace SomiodWebService.Migrations
{
	public partial class DataTable : DbMigration
	{
		public override void Up()
		{
			_ = CreateTable(
				"somiod.data",
				c => new
				{
					id = c.Int(nullable: false, identity: true),
					content = c.String(nullable: false, maxLength: 3),
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
			DropForeignKey("somiod.data", "parent", "somiod.containers");
			DropIndex("somiod.data", new[] { "name" });
			DropIndex("somiod.data", new[] { "parent" });
			DropTable("somiod.data");
		}
	}
}
