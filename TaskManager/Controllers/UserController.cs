using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Repository.IRepository;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        [HttpPost]
		public async Task<ActionResult<User>> Register(User user)
		{
			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult<User>> Login(User user) 
		{
			return Ok();
		}

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
			//change redirect
			return Ok();
		}

		[HttpDelete("{Id}")]
		public async Task<ActionResult<User>> DeleteUserAsync(int Id)
		{
			//отримуємо користувача
			User userToBeDeleted = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == Id);
			//перевіряємо чи даний користувач існує
			if (userToBeDeleted == null) { return NotFound(); }

			//logic for deleting all user`s task before deleting user
			
			//видаляємо в разі якщо користувач знайдений
			await _unitOfWork.User.DeleteAsync(userToBeDeleted);
			return NoContent();
		}

	}
}
