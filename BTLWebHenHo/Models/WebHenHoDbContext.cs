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
          public virtual DbSet<tbl_products> tbl_products { get; set; }
          public virtual DbSet<tbl_shares> tbl_shares { get; set; }
          public virtual DbSet<tbl_transaction> tbl_transaction { get; set; }
          public virtual DbSet<UserInfo> UserInfoes { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
          }
     }
}
