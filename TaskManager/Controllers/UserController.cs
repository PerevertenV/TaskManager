using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManager.Models;
using TaskManager.Repository.IRepository;
using TaskManager.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly JwtTokenService _tokenService;
		private readonly ILogger<User> _logger;

		public UserController(IUnitOfWork unitOfWork, 
			JwtTokenService tokenService, 
			ILogger<User> logger)
        {
            _unitOfWork = unitOfWork;
			_tokenService = tokenService;
			_logger = logger;
        }
		//api метод для отримання із БД всіх користувачів
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync() 
		{
			//отримуємо та одразу ж повертаємо елменти
			return Ok(await _unitOfWork.User.GetAllAsync());
		}
		//api метод для отримання одного унікального користувача за айді
		[HttpGet("{Id}")]
		public async Task<ActionResult<User>> GetUserAsync(int Id) 
		{
			//отримуємо користувача
			User userFromDb = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == Id);
			//перевіряємо чи даний користувач існує
			if (userFromDb == null) { return NotFound(); }  
			return Ok(userFromDb);

		}

        [HttpPost("register")]
		public async Task<ActionResult<User>> Register(User user)
		{
			//перевіряємо на валідність дані
			if (user == null || user.Username.IsNullOrEmpty() 
				|| user.Email.IsNullOrEmpty() 
				|| user.PasswordHash.IsNullOrEmpty()
				|| !user.Email.Contains('@')
				|| user.PasswordHash.Length < 6
				|| Regex.IsMatch(user.PasswordHash, "[a-zA-Z]"))
			{
				return BadRequest("Invalid register data");
			}
			//отримуємо всіх користувачів
			List<User> UsersFromDb = (await _unitOfWork.User.GetAllAsync()).ToList();
			//перевіряємо на збіг дані
			foreach(User users in UsersFromDb)
			{
				if(users == user || user.Username == users.Username || user.Email == users.Email) 
				{ 
					return BadRequest("This user already exist");
				}
			}
			//отримуємо звичайний пароль
			string ForHashingPassWord = user.PasswordHash;
			//хешуємо та перезаписуємо
			user.PasswordHash = _unitOfWork.User.PasswordHashCoder(ForHashingPassWord).ToString();
			//встанвлюємо змінні
			user.CreatedAt = DateTime.Now;
			user.UpdatedAt = DateTime.Now;
			//додаємо нового користувача
			await _unitOfWork.User.AddAsync(user);
			// генеруємо токен та передаємо його 
			string token = _tokenService.GenerateToken(user.Username, user.Email);
			_logger.LogInformation("registered user");
			return Ok(new { Token = token });
		}

		[HttpPost("login")]
		public async Task<ActionResult<User>> Login(string Login, string password) 
		{
			bool success = true;
			//перевіряємо на правельність передані дані
			if (Login.IsNullOrEmpty() || password.IsNullOrEmpty())
			{
				return BadRequest("Incorect user info");
			}
			//отримуємо всіх користувачів для порівняння даних
			List<User> UsersFromDb = (await _unitOfWork.User.GetAllAsync()).ToList();
			//перевірка 
			foreach (User users in UsersFromDb)
			{
				//отримуємо користувача коректного користувача
				if (users.Username == Login|| users.Email == Login)
				{
					//переводимо пароль
					string Decodet = await _unitOfWork.User.DecryptString(users.PasswordHash);
					//встановлюємо що юзер знайдений
					success = false;
					//звіряєм паролі
					if (password == Decodet)
					{
						//генеруємо токен та передаємо
						string token = _tokenService.GenerateToken(users.Username, users.Email);
						_logger.LogInformation("logined user");
						return Ok(new { Token = token });
					}
					else
					{
						return BadRequest("Incorrect password");
					}
				}
			}
			//у разі якщо користувач не був знайдений
			if (success)
			{
				return NotFound("Can't find user");
			}
			return Ok();
		}
		[Authorize]
		[HttpPut("{id}")]
		public async Task<ActionResult<User>> UpdateUserAsync(int id, User updatedUser)
		{
			//перевіряєм очи збігаєься id із id користувача дані якого оновлюються
			if (id != updatedUser.Id)
			{
				return BadRequest();
			}
			//Встановлюємо час останнього оновлення
			 updatedUser.UpdatedAt = DateTime.Now;
			//оновлюємо дані
			await _unitOfWork.User.UpdateAsync(updatedUser);
			//переадресовуємо до оновленого профілю
			return CreatedAtAction(nameof(GetUserAsync), new { id = updatedUser.Id }, updatedUser);
		}
		[Authorize]
		[HttpDelete("{Id}")]
		public async Task<ActionResult<User>> DeleteUserAsync(int Id)
		{
			//отримуємо користувача
			User userToBeDeleted = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == Id);
			//перевіряємо чи даний користувач існує
			if (userToBeDeleted == null) { return NotFound(); }

			//отримуємо задачі користувача якого маємо видаляти
			var tasks = (await _unitOfWork.Task.GetAllAsync(u => u.UserID == userToBeDeleted.Id)).ToList();
			//видаляємо всі задчі користувача перед видаленням самого користувача
			foreach (var task in tasks) 
			{
				await _unitOfWork.Task.DeleteAsync(task);
			}
			//видаляємо в разі якщо користувач знайдений
			await _unitOfWork.User.DeleteAsync(userToBeDeleted);
			_logger.LogInformation("user deleted successfully");
			return NoContent();
		}

	}
}
