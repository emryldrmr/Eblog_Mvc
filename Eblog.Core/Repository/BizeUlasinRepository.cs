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
   public class BizeUlasinRepository : IBizeUlasinRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public int Count()
        {
            return _context.bizeulasin.Count();
        }

        public void Delete(int id)
        {
            var bizeulasin = GetById(id);
            if (bizeulasin != null)
            {
                _context.bizeulasin.Remove(bizeulasin);
            }
        }

        public bizeulasin Get(Expression<Func<bizeulasin, bool>> expression)
        {
            return _context.bizeulasin.FirstOrDefault(expression);
        }

        public IEnumerable<bizeulasin> GetAll()
        {
            return _context.bizeulasin.Select(x => x);  // Tüm bizeulasinler dönecek
        }

        public bizeulasin GetById(int id)
        {
            return _context.bizeulasin.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<bizeulasin> GetMany(Expression<Func<bizeulasin, bool>> expression)
        {
            return _context.bizeulasin.Where(expression);
        }

        public void Insert(bizeulasin obj)
        {
            _context.bizeulasin.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(bizeulasin obj)
        {
            _context.bizeulasin.AddOrUpdate();
        }
    }
}
