using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Data;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
			_context = context;	
			this.dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T item)
		{
			await _context.AddAsync(item);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T item)
		{
			 _context.Remove(item);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter !=null ? query.Where(filter).ToList() : query.ToList();
		}

		public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter != null ? query.Where(filter).FirstOrDefault() : query.FirstOrDefault();
		}

		public async Task UpdateAsync(T item)
		{
			_context.Update(item);
			await _context.SaveChangesAsync();
		}
	}
}
