using BTLWebHenHo.EF.Services;
using BTLWebHenHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTLWebHenHo.Controllers
{
    public class Quizz_APIController : ApiController
    {
          WebHenHoDbContext db = new WebHenHoDbContext();
          [HttpGet]
          public List<tbl_quizz> GetQuizz()
          {
               List<tbl_quizz> ltq = db.tbl_quizz.ToList();
               return ltq;
          }
          [Route("api/Quizz_API/Update_qty_play")]
          [HttpPut]
          public string Update_qty_play(int id)
          {
               try
               {
                    F_tbl_quizz ftq = new F_tbl_quizz();
                    var row_upd = ftq.GetSingleByCondition(x=>x.id_quizz==id);
                    row_upd.qty_player++;
                    ftq.Update(row_upd);
                    return "Thanh cong";
               }
               catch
               {

               }
               return "That bai";

          }
    }
}
