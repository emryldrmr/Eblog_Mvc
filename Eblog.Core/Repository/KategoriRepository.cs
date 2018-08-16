using Eblog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Eblog.Data.Model;
using System.Linq.Expressions;

namespace Eblog.Core.Repository
{
   public class KategoriRepository : IKategoriRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public int Count()
        {
            return _context.kategori.Count();
        }

        public void Delete(int id)
        {
            var Kategori = GetById(id);
            if (Kategori != null)
            {
                _context.kategori.Remove(Kategori);
            }
        }

        public kategori Get(Expression<Func<kategori, bool>> expression)
        {
            return _context.kategori.FirstOrDefault(expression);
        }

        public IEnumerable<kategori> GetAll()
        {
            return _context.kategori.Select(x => x);  // Tüm kategoriler dönecek
        }

        public kategori GetById(int id)
        {
            return _context.kategori.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<kategori> GetMany(Expression<Func<kategori, bool>> expression)
        {
            return _context.kategori.Where(expression);
        }

        public void Insert(kategori obj)
        {
            _context.kategori.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(kategori obj)
        {
            _context.kategori.AddOrUpdate();
        }
    }
}
