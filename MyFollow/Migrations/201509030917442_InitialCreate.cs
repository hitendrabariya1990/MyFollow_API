namespace MyFollow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FollowProductOwners", newName: "FollowProducts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FollowProducts", newName: "FollowProductOwners");
        }
    }
}
