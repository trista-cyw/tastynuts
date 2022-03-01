namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        bannerName = c.String(nullable: false, maxLength: 200),
                        bannerStartDate = c.DateTime(nullable: false),
                        bannerEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        memberName = c.String(nullable: false, maxLength: 200),
                        memberBirth = c.DateTime(),
                        memberMobilePhone = c.String(maxLength: 50),
                        memberHomePhone = c.String(maxLength: 50),
                        memberMail = c.String(nullable: false, maxLength: 50),
                        memberPostcode = c.String(maxLength: 10),
                        memberAddress = c.String(maxLength: 200),
                        memberPassword = c.String(maxLength: 200),
                        memberPasswordSalt = c.String(maxLength: 5),
                        memberIsVerified = c.Boolean(nullable: false),
                        memberIsSNS = c.Boolean(nullable: false),
                        memberGUID = c.String(maxLength: 50),
                        memberGUIDValidTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        orderNumber = c.String(nullable: false, maxLength: 50),
                        orderDate = c.DateTime(nullable: false),
                        orderPayment = c.Int(nullable: false),
                        orderTotal = c.Int(nullable: false),
                        orderStatus = c.Int(nullable: false),
                        orderRcName = c.String(nullable: false, maxLength: 200),
                        orderRcMPhone = c.String(nullable: false, maxLength: 50),
                        orderRcHPhone = c.String(maxLength: 50),
                        orderRcMail = c.String(nullable: false, maxLength: 50),
                        orderRcPostCode = c.String(nullable: false, maxLength: 10),
                        orderRcAddress = c.String(nullable: false, maxLength: 200),
                        orderShipping = c.Int(nullable: false),
                        orderAmount = c.Int(nullable: false),
                        orderRemark = c.String(maxLength: 450),
                        memberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.memberId, cascadeDelete: true)
                .Index(t => t.memberId);
            
            CreateTable(
                "dbo.Order_info",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        productId = c.Int(nullable: false),
                        productAmount = c.Int(nullable: false),
                        productUnitPrice = c.Int(nullable: false),
                        orderSubtotal = c.Int(nullable: false),
                        orderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.orderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId)
                .Index(t => t.orderId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        productName = c.String(nullable: false, maxLength: 200),
                        productNumber = c.String(nullable: false, maxLength: 200),
                        productDescription = c.String(nullable: false, maxLength: 450),
                        productSummary = c.String(nullable: false, maxLength: 450),
                        productOriPrice = c.Int(nullable: false),
                        productSpePrice = c.Int(nullable: false),
                        productIsLaunched = c.Boolean(nullable: false),
                        productClass = c.Int(nullable: false),
                        productDate = c.DateTime(nullable: false),
                        productServing = c.Int(nullable: false),
                        productIncluding = c.Int(nullable: false),
                        productSCalories = c.Int(nullable: false),
                        productGCalories = c.Int(nullable: false),
                        productSProtein = c.Int(nullable: false),
                        productGProtein = c.Int(nullable: false),
                        productSFat = c.Int(nullable: false),
                        productGFat = c.Int(nullable: false),
                        productSSaturatedFat = c.Int(nullable: false),
                        productGSaturatedFat = c.Int(nullable: false),
                        productSTransFat = c.Int(nullable: false),
                        productGTransFat = c.Int(nullable: false),
                        productSCarbohydrate = c.Int(nullable: false),
                        productGCarbohydrate = c.Int(nullable: false),
                        productSSugar = c.Int(nullable: false),
                        productGSugar = c.Int(nullable: false),
                        productSNa = c.Int(nullable: false),
                        productGNa = c.Int(nullable: false),
                        productImgCover = c.String(nullable: false, maxLength: 200),
                        productImg02 = c.String(nullable: false, maxLength: 200),
                        productImg03 = c.String(nullable: false, maxLength: 200),
                        productImg04 = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        recipeTitle = c.String(nullable: false, maxLength: 200),
                        recipeSummary = c.String(nullable: false, maxLength: 450),
                        recipeIngredient = c.String(nullable: false, maxLength: 450),
                        recipeStep01 = c.String(nullable: false, maxLength: 450),
                        recipeStep02 = c.String(maxLength: 450),
                        recipeStep03 = c.String(maxLength: 450),
                        recipeCover = c.String(nullable: false, maxLength: 200),
                        recipeDate = c.DateTime(nullable: false),
                        recipeIsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order_info", "productId", "dbo.Products");
            DropForeignKey("dbo.Order_info", "orderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "memberId", "dbo.Members");
            DropIndex("dbo.Order_info", new[] { "orderId" });
            DropIndex("dbo.Order_info", new[] { "productId" });
            DropIndex("dbo.Orders", new[] { "memberId" });
            DropTable("dbo.Recipes");
            DropTable("dbo.Products");
            DropTable("dbo.Order_info");
            DropTable("dbo.Orders");
            DropTable("dbo.Members");
            DropTable("dbo.Banners");
        }
    }
}
