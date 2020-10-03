
using BTLWebHenHo.Models;
using System;

namespace BTLWebHenHo.EF.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
          WebHenHoDbContext Init();
    }
}