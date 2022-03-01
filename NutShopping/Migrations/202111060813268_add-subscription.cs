namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        subscriptionName = c.String(nullable: false, maxLength: 200),
                        subscriptionNumber = c.String(nullable: false, maxLength: 200),
                        subscriptionDescription = c.String(nullable: false, maxLength: 450),
                        subscriptionSummary = c.String(nullable: false, maxLength: 450),
                        subscriptionPrice = c.Int(nullable: false),
                        subscriptionIsLaunched = c.Boolean(nullable: false),
                        subscriptionDate = c.DateTime(nullable: false),
                        subscriptioncCycle = c.Int(nullable: false),
                        subscriptionServing = c.Int(nullable: false),
                        subscriptionIncluding = c.Int(nullable: false),
                        subscriptionSCalories = c.Int(nullable: false),
                        subscriptionGCalories = c.Int(nullable: false),
                        subscriptionSProtein = c.Int(nullable: false),
                        subscriptionGProtein = c.Int(nullable: false),
                        subscriptionSFat = c.Int(nullable: false),
                        subscriptionGFat = c.Int(nullable: false),
                        subscriptionSSaturatedFat = c.Int(nullable: false),
                        subscriptionGSaturatedFat = c.Int(nullable: false),
                        subscriptionSTransFat = c.Int(nullable: false),
                        subscriptionGTransFat = c.Int(nullable: false),
                        subscriptionSCarbohydrate = c.Int(nullable: false),
                        subscriptionGCarbohydrate = c.Int(nullable: false),
                        subscriptionSSugar = c.Int(nullable: false),
                        subscriptionGSugar = c.Int(nullable: false),
                        subscriptionSNa = c.Int(nullable: false),
                        subscriptionGNa = c.Int(nullable: false),
                        subscriptionImgCover = c.String(nullable: false, maxLength: 200),
                        subscriptionImg02 = c.String(maxLength: 200),
                        subscriptionImg03 = c.String(nullable: false, maxLength: 200),
                        subscriptionImg04 = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subscriptions");
        }
    }
}
