namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproduct_deleteImg : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "productImg02");
            DropColumn("dbo.Products", "productImg03");
            DropColumn("dbo.Products", "productImg04");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "productImg04", c => c.String(maxLength: 200));
            AddColumn("dbo.Products", "productImg03", c => c.String(maxLength: 200));
            AddColumn("dbo.Products", "productImg02", c => c.String(maxLength: 200));
        }
    }
}
