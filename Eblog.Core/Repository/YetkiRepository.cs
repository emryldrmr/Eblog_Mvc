using Eblog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Migrations; //AddorUpdate için gerekli
using Eblog.Data.Model;

namespace Eblog.Core.Repository
{
    public class YetkiRepository : IYetkiRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public int Count()
        {
            return _context.yetki.Count();
        }

        public void Delete(int id)
        {
            var Yetki = GetById(id);
            if (Yetki != null)
            {
                _context.yetki.Remove(Yetki);
            }
        }

        public yetki Get(Expression<Func<yetki, bool>> expression)
        {
            return _context.yetki.FirstOrDefault(expression);
        }

        public IEnumerable<yetki> GetAll()
        {
            return _context.yetki.Select(x => x);  // Tüm Yetkiler dönecek
        }

        public yetki GetById(int id)
        {
            return _context.yetki.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<yetki> GetMany(Expression<Func<yetki, bool>> expression)
        {
            return _context.yetki.Where(expression);
        }

        public void Insert(yetki obj)
        {
            _context.yetki.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(yetki obj)
        {
            _context.yetki.AddOrUpdate();
        }
    }
}
