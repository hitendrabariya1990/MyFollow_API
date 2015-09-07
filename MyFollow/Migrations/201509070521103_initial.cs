namespace MyFollow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FollowProducts", "Euid", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.FollowProducts", new[] { "Euid" });
            DropIndex("dbo.FollowProducts", new[] { "ProductId" });
            AddColumn("dbo.FollowProducts", "MProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.FollowProducts", "MProductId");
            AddForeignKey("dbo.FollowProducts", "MProductId", "dbo.MainProducts", "Id", cascadeDelete: true);
            DropColumn("dbo.FollowProducts", "ProductId");
            DropColumn("dbo.FollowProducts", "Flag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FollowProducts", "Flag", c => c.Boolean(nullable: false));
            AddColumn("dbo.FollowProducts", "ProductId", c => c.Int(nullable: false));
            DropForeignKey("dbo.FollowProducts", "MProductId", "dbo.MainProducts");
            DropIndex("dbo.FollowProducts", new[] { "MProductId" });
            DropColumn("dbo.FollowProducts", "MProductId");
            CreateIndex("dbo.FollowProducts", "ProductId");
            CreateIndex("dbo.FollowProducts", "Euid");
            AddForeignKey("dbo.FollowProducts", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FollowProducts", "Euid", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
