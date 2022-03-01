namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproducts_NutritionFacts_notrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "productServing", c => c.Int());
            AlterColumn("dbo.Products", "productIncluding", c => c.Int());
            AlterColumn("dbo.Products", "productSCalories", c => c.Int());
            AlterColumn("dbo.Products", "productGCalories", c => c.Int());
            AlterColumn("dbo.Products", "productSProtein", c => c.Int());
            AlterColumn("dbo.Products", "productGProtein", c => c.Int());
            AlterColumn("dbo.Products", "productSFat", c => c.Int());
            AlterColumn("dbo.Products", "productGFat", c => c.Int());
            AlterColumn("dbo.Products", "productSSaturatedFat", c => c.Int());
            AlterColumn("dbo.Products", "productGSaturatedFat", c => c.Int());
            AlterColumn("dbo.Products", "productSTransFat", c => c.Int());
            AlterColumn("dbo.Products", "productGTransFat", c => c.Int());
            AlterColumn("dbo.Products", "productSCarbohydrate", c => c.Int());
            AlterColumn("dbo.Products", "productGCarbohydrate", c => c.Int());
            AlterColumn("dbo.Products", "productSSugar", c => c.Int());
            AlterColumn("dbo.Products", "productGSugar", c => c.Int());
            AlterColumn("dbo.Products", "productSNa", c => c.Int());
            AlterColumn("dbo.Products", "productGNa", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "productGNa", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSNa", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGSugar", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSSugar", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGCarbohydrate", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSCarbohydrate", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGTransFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSTransFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGSaturatedFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSSaturatedFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSFat", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGProtein", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSProtein", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productGCalories", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productSCalories", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productIncluding", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "productServing", c => c.Int(nullable: false));
        }
    }
}
