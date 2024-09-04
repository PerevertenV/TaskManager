using System.Security.Cryptography;
using System.Text;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context): base(context)
		{
			_context = context;
        }

        public async Task<string> DecryptString(string encryptedText)
		{
			return await System.Threading.Tasks.Task.Run(() =>
			{
				byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
				byte[] decrypted = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
				return Encoding.Unicode.GetString(decrypted);
			});
		}

		public async Task<string> PasswordHashCoder(string password)
		{
			return await System.Threading.Tasks.Task.Run(() =>
			{
				byte[] bytes = Encoding.Unicode.GetBytes(password);
				byte[] encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
				return Convert.ToBase64String(encrypted);
			});
		}
	}
}
