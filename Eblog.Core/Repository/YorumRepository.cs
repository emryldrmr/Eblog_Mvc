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
    public class YorumRepository : IYorumRepository
    {
        private readonly eblogdb _context = new eblogdb();

        public int Count()
        {
            return _context.yorum.Count();
        }

        public void Delete(int id)
        {
            var Yorum = GetById(id);
            if (Yorum != null)
            {
                _context.yorum.Remove(Yorum);
            }
        }

        public yorum Get(Expression<Func<yorum, bool>> expression)
        {
            return _context.yorum.FirstOrDefault(expression);
        }

        public IEnumerable<yorum> GetAll()
        {
            return _context.yorum.Select(x => x);  // Tüm Yorumler dönecek
        }

        public yorum GetById(int id)
        {
            return _context.yorum.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<yorum> GetMany(Expression<Func<yorum, bool>> expression)
        {
            return _context.yorum.Where(expression);
        }

        public void Insert(yorum obj)
        {
            _context.yorum.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(yorum obj)
        {
            _context.yorum.AddOrUpdate();
        }
    }
}
