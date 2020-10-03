

namespace BTLWebHenHo.EF.Model
{
     using System;
     using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
     using System.Web;
     public partial class Image
     {
          public int ImageID { get; set; }
          public string Title { get; set; }
          [DisplayName("Upload File")]
          public string ImagePath { get; set; }

          public HttpPostedFileBase ImageFile { get; set; }
     }
}