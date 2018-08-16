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
    public class MakaleRepository : IMakaleRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public IEnumerable<makale> GetAll()
        {
            return _context.makale.Select(x => x); // Tüm haberler dönecek
        }

        public makale GetById(int id)
        {
            return _context.makale.FirstOrDefault(x => x.ID == id);
        }

        public makale Get(System.Linq.Expressions.Expression<Func<makale, bool>> expression)
        {
            return _context.makale.FirstOrDefault(expression);
        }

        public IQueryable<makale> GetMany(System.Linq.Expressions.Expression<Func<makale, bool>> expression)
        {
            return _context.makale.Where(expression);
        }

        public void Insert(makale obj)
        {
            _context.makale.Add(obj);
        }

        public void Update(makale obj)
        {
            _context.makale.AddOrUpdate();
        }

        public void Delete(int id)
        {
            var Makale = GetById(id);
            if (Makale != null)
            {
                _context.makale.Remove(Makale);
            }
        }

        public int Count()
        {
            return _context.makale.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
