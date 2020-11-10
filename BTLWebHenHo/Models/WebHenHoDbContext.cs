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

          public virtual DbSet<new_imgage> new_imgage { get; set; }
          public virtual DbSet<Profile_User> Profile_User { get; set; }
          public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
          public virtual DbSet<tbl_admin> tbl_admin { get; set; }
          public virtual DbSet<tbl_blogs> tbl_blogs { get; set; }
          public virtual DbSet<tbl_catalogs> tbl_catalogs { get; set; }
          public virtual DbSet<tbl_category> tbl_category { get; set; }
          public virtual DbSet<tbl_chat> tbl_chat { get; set; }
          public virtual DbSet<tbl_codes> tbl_codes { get; set; }
          public virtual DbSet<tbl_collects> tbl_collects { get; set; }
          public virtual DbSet<tbl_history_chat> tbl_history_chat { get; set; }
          public virtual DbSet<tbl_image_links> tbl_image_links { get; set; }
          public virtual DbSet<tbl_order> tbl_order { get; set; }
          public virtual DbSet<tbl_permission> tbl_permission { get; set; }
          public virtual DbSet<tbl_products> tbl_products { get; set; }
          public virtual DbSet<tbl_quizz> tbl_quizz { get; set; }
          public virtual DbSet<tbl_shares> tbl_shares { get; set; }
          public virtual DbSet<tbl_transaction> tbl_transaction { get; set; }
          public virtual DbSet<tbl_user_permission> tbl_user_permission { get; set; }
          public virtual DbSet<tbl_User_Type> tbl_User_Type { get; set; }
          public virtual DbSet<UserInfo> UserInfoes { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
               modelBuilder.Entity<new_imgage>()
                   .Property(e => e.new_img)
                   .IsUnicode(false);

               modelBuilder.Entity<Profile_User>()
                   .Property(e => e.birthday)
                   .IsUnicode(false);

               modelBuilder.Entity<tbl_permission>()
                   .Property(e => e.name)
                   .IsUnicode(false);

               modelBuilder.Entity<tbl_quizz>()
                   .Property(e => e.img_tittle)
                   .IsUnicode(false);

               modelBuilder.Entity<UserInfo>()
                   .Property(e => e.con_ID)
                   .IsUnicode(false);
          }
     }
}
