namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecipt_summary_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recipes", "recipeSummary", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipes", "recipeSummary", c => c.String(nullable: false, maxLength: 450));
        }
    }
}
