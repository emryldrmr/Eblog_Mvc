using Eblog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eblog.Core.Infrastructure
{
    public interface IEtiketRepository : IRepository<etiket>
    {
        IQueryable<etiket> Etiketler(string[] etiketler);

        void EtiketEkle(int MakaleID, string Etiket);

        void MakaleEtiketEkle(int MakaleID, string[] etiketler);
    }
}
