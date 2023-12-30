using System.Data.Entity.Migrations;

namespace SomiodWebService.Migrations
{
	public partial class ContainerTable : DbMigration
	{
		public override void Up()
		{
			_ = CreateTable(
				"somiod.containers",
				c => new
				{
					id = c.Int(nullable: false, identity: true),
					parent = c.Int(nullable: false),
					name = c.String(nullable: false, maxLength: 255),
					creation_dt = c.String(nullable: false),
				})
				.PrimaryKey(t => t.id)
				.ForeignKey("somiod.applications", t => t.parent, cascadeDelete: true)
				.Index(t => t.parent)
				.Index(t => t.name, unique: true);

			CreateIndex("somiod.applications", "name", unique: true);
		}

		public override void Down()
		{
			DropForeignKey("somiod.containers", "parent", "somiod.applications");
			DropIndex("somiod.containers", new[] { "name" });
			DropIndex("somiod.containers", new[] { "parent" });
			DropIndex("somiod.applications", new[] { "name" });
			DropTable("somiod.containers");
		}
	}
}
