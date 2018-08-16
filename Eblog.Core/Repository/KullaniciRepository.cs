using Eblog.Core.Infrastructure;
using Eblog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace Eblog.Core.Repository
{
    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public IEnumerable<Data.Model.kullanici> GetAll()
        {
            return _context.kullanici.Select(x => x); // Tüm haberler dönecek
        }

        public Data.Model.kullanici GetById(int id)
        {
            return _context.kullanici.FirstOrDefault(x => x.ID == id);
        }

        public Data.Model.kullanici Get(System.Linq.Expressions.Expression<Func<Data.Model.kullanici, bool>> expression)
        {
            return _context.kullanici.FirstOrDefault(expression);
        }

        public IQueryable<Data.Model.kullanici> GetMany(System.Linq.Expressions.Expression<Func<Data.Model.kullanici, bool>> expression)
        {
            return _context.kullanici.Where(expression);
        }

        public void Insert(Data.Model.kullanici obj)
        {
            _context.kullanici.Add(obj);
        }

        public void Update(Data.Model.kullanici obj)
        {
            _context.kullanici.AddOrUpdate();
        }

        public void Delete(int id)
        {
            var Kullanici = GetById(id);
            if (Kullanici != null)
            {
                _context.kullanici.Remove(Kullanici);
            }
        }

        public int Count()
        {
            return _context.kullanici.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Data.Model.kullanici KullaniciBul(string Email)
        {
            return _context.kullanici.FirstOrDefault(x => x.Email == Email);
        }
    }
}
