namespace NutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editsubscription_delete_subscriptionImg : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Subscriptions", "subscriptionImg02");
            DropColumn("dbo.Subscriptions", "subscriptionImg03");
            DropColumn("dbo.Subscriptions", "subscriptionImg04");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subscriptions", "subscriptionImg04", c => c.String(maxLength: 200));
            AddColumn("dbo.Subscriptions", "subscriptionImg03", c => c.String(maxLength: 200));
            AddColumn("dbo.Subscriptions", "subscriptionImg02", c => c.String(maxLength: 200));
        }
    }
}
