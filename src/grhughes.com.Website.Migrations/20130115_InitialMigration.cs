namespace grhughes.com.Website.Migrations
{
  using FluentMigrator;

  [Migration(20130115)]
  public class InitialMigration : Migration
  {
    public override void Up()
    {
      Create.Table("Users")
        .WithColumn("Id").AsInt32().PrimaryKey().Indexed().Identity()
        .WithColumn("UserId").AsGuid()
        .WithColumn("Email").AsString(int.MaxValue)
        .WithColumn("Password").AsCustom("nvarchar(MAX)")
        .WithColumn("Salt").AsCustom("nvarchar(MAX)")
        .WithColumn("AppsCode").AsCustom("nvarchar(MAX)");

      Create.Table("BlogPosts")
        .WithColumn("Id").AsInt32().PrimaryKey().Indexed().Identity()
        .WithColumn("Title").AsCustom("nvarchar(MAX)")
        .WithColumn("Slug").AsCustom("nvarchar(MAX)")
        .WithColumn("PublishDate").AsDateTime()
        .WithColumn("CreationDate").AsDateTime()
        .WithColumn("Published").AsBoolean()
        .WithColumn("Content").AsCustom("nvarchar(MAX)")
        .WithColumn("UserId").AsInt32();

      Create.ForeignKey("FK_Blog_User").FromTable("BlogPosts").ForeignColumn("UserId").ToTable("Users").PrimaryColumn(
        "Id");
    }

    public override void Down()
    {
      Delete.ForeignKey("FK_Blog_User");
      Delete.Table("Users");
      Delete.Table("BlogPosts");
    }
  }
}