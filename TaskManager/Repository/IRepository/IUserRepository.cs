using TaskManager.Models;

namespace TaskManager.Repository.IRepository
{
	public interface IUserRepository: IRepository<User>
	{
	    Task<string> PasswordHashCoder(string password);
		Task<string> DecryptString(string encryptedText);
	}
}
