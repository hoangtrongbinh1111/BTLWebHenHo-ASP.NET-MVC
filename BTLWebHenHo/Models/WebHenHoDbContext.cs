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

          public virtual DbSet<UserInfo> UserInfoes { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
               modelBuilder.Entity<UserInfo>()
                   .Property(e => e.username)
                   .IsUnicode(false);

               modelBuilder.Entity<UserInfo>()
                   .Property(e => e.passw)
                   .IsUnicode(false);
          }
     }
}
