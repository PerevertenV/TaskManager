using TaskManager.Data;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
	public class TaskRepository: Repository<Task>, ITaskRepository
	{
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
