namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editsubscription_img03_notrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Subscriptions", "subscriptionImg03", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subscriptions", "subscriptionImg03", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
