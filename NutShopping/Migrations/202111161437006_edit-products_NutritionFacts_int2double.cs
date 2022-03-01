namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproducts_NutritionFacts_int2double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "productSCalories", c => c.Double());
            AlterColumn("dbo.Products", "productGCalories", c => c.Double());
            AlterColumn("dbo.Products", "productSProtein", c => c.Double());
            AlterColumn("dbo.Products", "productGProtein", c => c.Double());
            AlterColumn("dbo.Products", "productSFat", c => c.Double());
            AlterColumn("dbo.Products", "productGFat", c => c.Double());
            AlterColumn("dbo.Products", "productSSaturatedFat", c => c.Double());
            AlterColumn("dbo.Products", "productGSaturatedFat", c => c.Double());
            AlterColumn("dbo.Products", "productSTransFat", c => c.Double());
            AlterColumn("dbo.Products", "productGTransFat", c => c.Double());
            AlterColumn("dbo.Products", "productSCarbohydrate", c => c.Double());
            AlterColumn("dbo.Products", "productGCarbohydrate", c => c.Double());
            AlterColumn("dbo.Products", "productSSugar", c => c.Double());
            AlterColumn("dbo.Products", "productGSugar", c => c.Double());
            AlterColumn("dbo.Products", "productSNa", c => c.Double());
            AlterColumn("dbo.Products", "productGNa", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "productGNa", c => c.Int());
            AlterColumn("dbo.Products", "productSNa", c => c.Int());
            AlterColumn("dbo.Products", "productGSugar", c => c.Int());
            AlterColumn("dbo.Products", "productSSugar", c => c.Int());
            AlterColumn("dbo.Products", "productGCarbohydrate", c => c.Int());
            AlterColumn("dbo.Products", "productSCarbohydrate", c => c.Int());
            AlterColumn("dbo.Products", "productGTransFat", c => c.Int());
            AlterColumn("dbo.Products", "productSTransFat", c => c.Int());
            AlterColumn("dbo.Products", "productGSaturatedFat", c => c.Int());
            AlterColumn("dbo.Products", "productSSaturatedFat", c => c.Int());
            AlterColumn("dbo.Products", "productGFat", c => c.Int());
            AlterColumn("dbo.Products", "productSFat", c => c.Int());
            AlterColumn("dbo.Products", "productGProtein", c => c.Int());
            AlterColumn("dbo.Products", "productSProtein", c => c.Int());
            AlterColumn("dbo.Products", "productGCalories", c => c.Int());
            AlterColumn("dbo.Products", "productSCalories", c => c.Int());
        }
    }
}
