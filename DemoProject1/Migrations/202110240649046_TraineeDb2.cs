namespace DemoProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TraineeDb2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Trainees", new[] { "TraineeId" });
            DropPrimaryKey("dbo.Trainees");
            AlterColumn("dbo.Trainees", "TraineeId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Trainees", "TraineeId");
            CreateIndex("dbo.Trainees", "TraineeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Trainees", new[] { "TraineeId" });
            DropPrimaryKey("dbo.Trainees");
            AlterColumn("dbo.Trainees", "TraineeId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Trainees", "Name");
            CreateIndex("dbo.Trainees", "TraineeId");
        }
    }
}
