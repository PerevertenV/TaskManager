using TaskManager.Data;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public IUserRepository User { get; private set; }
		public ITaskRepository Task { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
			_context = context;
			User = new UserRepository(_context);
			Task = new TaskRepository(_context);
        }


    }
}
