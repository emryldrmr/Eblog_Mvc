using Eblog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations; // AddOrUpdate için gerekli
using Eblog.Data.Model;


namespace Eblog.Core.Repository
{
    public class EtiketRepository : IEtiketRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public IEnumerable<etiket> GetAll()
        {
            return _context.etiket.Select(x => x); // Tüm Etiketler dönecek
        }

        public Data.Model.etiket GetById(int id)
        {
            return _context.etiket.FirstOrDefault(x => x.ID == id);
        }

        public Data.Model.etiket Get(System.Linq.Expressions.Expression<Func<etiket, bool>> expression)
        {
            return _context.etiket.FirstOrDefault(expression);
        }

        public IQueryable<etiket> GetMany(System.Linq.Expressions.Expression<Func<etiket, bool>> expression)
        {
            return _context.etiket.Where(expression);
        }

        public void Insert(etiket obj)
        {
            _context.etiket.Add(obj);
        }

        public void Update(etiket obj)
        {
            _context.etiket.AddOrUpdate();
        }

        public void Delete(int id)
        {
            var Etiket = GetById(id);
            if (Etiket != null)
            {
                _context.etiket.Remove(Etiket);
            }
        }

        public int Count()
        {
            return _context.etiket.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public IQueryable<Data.Model.etiket> Etiketler(string[] etiketler)
        {
            return _context.etiket.Where(x => etiketler.Contains(x.EtiketAdi));
        }

        public void EtiketEkle(int MakaleID, string Etiket)
        {

            if (Etiket != null && Etiket != "")
            {
                string[] Etikets = Etiket.Split(',');
                foreach (var tag in Etikets)
                {
                    etiket etiket = this.Get(x => x.EtiketAdi.ToLower() == tag.ToLower().Trim());
                    if (etiket == null)
                    {
                        etiket = new etiket();
                        etiket.EtiketAdi = tag;
                        this.Insert(etiket);
                        this.Save();
                    }

                }
                this.MakaleEtiketEkle(MakaleID, Etikets);
            }

        }


        public void MakaleEtiketEkle(int MakaleID, string[] etiketler)
        {
            var Makale = _context.makale.FirstOrDefault(x => x.ID == MakaleID);
            var gelenEtiket = this.Etiketler(etiketler);

            Makale.etiket.Clear();
            gelenEtiket.ToList().ForEach(etiket => Makale.etiket.Add(etiket));
            _context.SaveChanges();
        }
    }
}
