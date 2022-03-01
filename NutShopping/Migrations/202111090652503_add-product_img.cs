namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproduct_img : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Product_img",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        productImgName = c.String(nullable: false, maxLength: 200),
                        productImgDate = c.DateTime(nullable: false),
                        productId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product_img", "productId", "dbo.Products");
            DropIndex("dbo.Product_img", new[] { "productId" });
            DropTable("dbo.Product_img");
        }
    }
}
