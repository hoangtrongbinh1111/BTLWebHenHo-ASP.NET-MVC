namespace BTLWebHenHo.Models
{
     using System;
     using System.Data.Entity;
     using System.ComponentModel.DataAnnotations.Schema;
     using System.Linq;

     public partial class WebHenHoDbContext : DbContext
     {
          public WebHenHoDbContext()
              : base("name=WebHenHoDbContext")
          {
          }

          public virtual DbSet<Profile_User> Profile_User { get; set; }
          public virtual DbSet<UserInfo> UserInfoes { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
          }
     }
}
