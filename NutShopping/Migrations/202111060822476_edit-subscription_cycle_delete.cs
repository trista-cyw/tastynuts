namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editsubscription_cycle_delete : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Subscriptions", "subscriptioncCycle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subscriptions", "subscriptioncCycle", c => c.Int(nullable: false));
        }
    }
}
