using Eblog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eblog.Core.Infrastructure
{
    public interface IKullaniciRepository : IRepository<kullanici>
    {
        kullanici KullaniciBul(string Email);
    }
}
