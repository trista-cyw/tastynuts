namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addorder_subinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order_subinfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        subscriptiontId = c.Int(nullable: false),
                        subscriptioncCycle = c.Int(nullable: false),
                        subscriptionPrice = c.Int(nullable: false),
                        orderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.orderId, cascadeDelete: true)
                .ForeignKey("dbo.Subscriptions", t => t.subscriptiontId, cascadeDelete: true)
                .Index(t => t.subscriptiontId)
                .Index(t => t.orderId);
            
            AddColumn("dbo.Orders", "orderIsSubscription", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order_subinfo", "subscriptiontId", "dbo.Subscriptions");
            DropForeignKey("dbo.Order_subinfo", "orderId", "dbo.Orders");
            DropIndex("dbo.Order_subinfo", new[] { "orderId" });
            DropIndex("dbo.Order_subinfo", new[] { "subscriptiontId" });
            DropColumn("dbo.Orders", "orderIsSubscription");
            DropTable("dbo.Order_subinfo");
        }
    }
}
