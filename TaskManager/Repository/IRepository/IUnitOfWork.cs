namespace TaskManager.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IUserRepository User { get; }
		ITaskRepository Task { get; }
	}
}
