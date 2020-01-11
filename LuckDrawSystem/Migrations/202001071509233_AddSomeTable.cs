namespace LuckDrawSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrizeWinners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrizeType = c.String(nullable: false),
                        WinningNumber = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WinningNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        LuckyNo = c.Int(nullable: false),
                        Byhand = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WinningNumbers");
            DropTable("dbo.PrizeWinners");
        }
    }
}
