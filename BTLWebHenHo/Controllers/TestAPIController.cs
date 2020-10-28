using BTLWebHenHo.EF.Services;
using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTLWebHenHo.Controllers
{
    public class TestAPIController : ApiController
    {
          WebHenHoDbContext db = new WebHenHoDbContext();
          [HttpGet]
          public List<tbl_blogs> getBlog()
          {
               List<tbl_blogs> tb = db.tbl_blogs.ToList();
               return tb;
          }
          [HttpGet]
          public tbl_blogs getBlog(int id)
          {
               tbl_blogs tb = db.tbl_blogs.FirstOrDefault(x=>x.id_blog==id);
               return tb;
          }
          [HttpGet]
          public tbl_blogs getBlog_Category(int category)
          {
               tbl_blogs tb = db.tbl_blogs.Where(x => x.id_category == category).FirstOrDefault();
               return tb;
          }
          [HttpPost]
          public bool AddCatalog(string name)
          {
               try
               {
                    tbl_catalogs tc = new tbl_catalogs();
                    tc.name = name;
                    db.tbl_catalogs.Add(tc);
                    db.SaveChanges();
                    return true;
               }
               catch
               {

               }
               return false;
          }
          [HttpPut]
          public bool FixCatalog(int id)
          {
               try
               {
                    F_tbl_catalog ftc = new F_tbl_catalog();
                    var row_upd = ftc.GetSingleByCondition(x => x.id_catalog == id);
                    row_upd.name = "Binh_Fixed";
                    ftc.Update(row_upd);
                    return true;
               }
               catch
               {

               }
               return false;
          }
          [HttpDelete]
          public bool DelCatalog(int id)
          {
               try
               {
                    
                    var tc = new tbl_catalogs { id_catalog = id };
                    db.Entry(tc).State = EntityState.Deleted;
                    db.SaveChanges();
                    return true;
               }
               catch
               {

               }
               return false;
          }
    }
}
