namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproduct_Img_notrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "productImgCover", c => c.String(maxLength: 200));
            AlterColumn("dbo.Products", "productImg02", c => c.String(maxLength: 200));
            AlterColumn("dbo.Products", "productImg03", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "productImg03", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Products", "productImg02", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Products", "productImgCover", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
