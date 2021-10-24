namespace DemoProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TraineeDb1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Trainees");
            AddPrimaryKey("dbo.Trainees", "Name");
            DropColumn("dbo.Trainees", "Id");
            DropColumn("dbo.Trainees", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainees", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Trainees", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Trainees");
            AddPrimaryKey("dbo.Trainees", "Id");
        }
    }
}
