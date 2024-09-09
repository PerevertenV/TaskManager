using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Repository.IRepository;
using TaskManager.StaticData;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TaskController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<Models.Task> _logger;

		public TaskController(IUnitOfWork unitOfWork, ILogger<Models.Task> logger)
        {
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[HttpGet("all/{userId}")]
		public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasksAsync(int userId, 
			[FromQuery] string? sortBy,
			[FromQuery] string? filterBy,
			[FromQuery] string? filterOption,
			[FromQuery] DateTime? timePoint)
		{
			//перервіряємо чи правельно передали дані
			if(userId == null) { return BadRequest("Incorrect user info"); }
			//отримуємо всі завдання із БД
			List<Models.Task> tasksFromDb = (await _unitOfWork.Task.GetAllAsync(u => 
				u.UserID == userId)).ToList();
			//Превіряємо чи користувач має завдання
			if (tasksFromDb.IsNullOrEmpty()) { return NotFound("Current user doesn't have any tasks"); }
			//сортування
			if (!sortBy.IsNullOrEmpty())
			{
				//переводимо всі символи до нижнього регістру щоб коректно порівняти
				switch (sortBy.ToLower())
				{
					//якщо запит на дату
					case "duedate":
						tasksFromDb = tasksFromDb.OrderBy(t => t.DueDate).ToList();
						break;
					//якщо запит на преорітет
					case "priority":
						tasksFromDb = tasksFromDb.OrderBy(t => t.Priority).ToList();
						break;
					//якщо не коректно вказано сортуємо по дефолту за датою 
					default:
						tasksFromDb = tasksFromDb.OrderBy(t => t.DueDate).ToList();
						break;
				}
			}
			//фільрація
			if (!filterOption.IsNullOrEmpty()) 
			{
				//виконання за умови фільтра
				switch (filterBy.ToLower()) 
				{
					//якщо фільтр пріоритет
					case "priority":
						//вибираємо умови та за допомоги LINQ перезаписуємо
						if(filterOption.ToLower() == SD.Priority_Low.ToLower()) 
						{
							tasksFromDb = tasksFromDb.Where(u => 
							u.Priority == SD.Priority_Low).ToList();
						}
						else if (filterOption.ToLower() == SD.Priority_Medium.ToLower())
						{
							tasksFromDb = tasksFromDb.Where(u => 
								u.Priority == SD.Priority_Medium).ToList();
						}
						else if (filterOption.ToLower() == SD.Priority_High.ToLower())
						{
							tasksFromDb = tasksFromDb.Where(u =>
								u.Priority == SD.Priority_High).ToList();
						}
						else { return NotFound("Incorect filter option"); }
						break;
					//якщо фільтр статус 
					case "status":
						//вибираємо умови та за допомоги LINQ фільтруємо
						if (filterOption.ToLower() == SD.Status_Pending.ToLower())
						{
							tasksFromDb = tasksFromDb.Where(u =>
							u.Status == SD.Status_Pending).ToList();
						}
						else if (filterOption.ToLower() == SD.Status_InProgress.ToLower())
						{
							tasksFromDb = tasksFromDb.Where(u =>
								u.Status == SD.Status_InProgress).ToList();
						}
						else if (filterOption.ToLower() == SD.Status_Completed.ToLower())
						{
							tasksFromDb = tasksFromDb.Where(u =>
								u.Status == SD.Status_Completed).ToList();
						}
						else { return NotFound("Incorect filter option"); }
						break;
					//якщо вставили фільт по даті
					//вибираємо умови та за допомоги LINQ фільтруємо
					case "duedate":
						//перевіряємо чи вставнвлена точка відносного порівняння
						if(timePoint == null) 
						{
							return NotFound("Time point weren't set");
						}
						if(filterOption.ToLower() == "before") 
						{
							tasksFromDb = tasksFromDb.Where(u =>
								u.DueDate > timePoint).ToList();
						}
						else if(filterOption.ToLower() == "after") 
						{
							tasksFromDb = tasksFromDb.Where(u =>
								u.DueDate < timePoint).ToList();
						}
						else { return NotFound("Incorect filter option"); }
						break;
				}
			}
			//відфільтрований(або ж ні) відсортовантй(або ж ні) список завдань передається
			_logger.LogInformation("all tasks releted to one user get successfully");
			return Ok(tasksFromDb);
		}

		[HttpGet("{Id}")]
		public async Task<ActionResult<Models.Task>> GetTaskAsync(int Id)
		{
			//отримуємо завдння
			var taskFromDb = await _unitOfWork.Task.GetFirstOrDefaultAsync(u => u.Id == Id);
			//перевіряємо чи дане завдання існує
			if (taskFromDb == null) { return NotFound(); }
			return Ok(taskFromDb);
		}

		[HttpPost]
		public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task, int UserId) 
		{
			//підв'язуємо користувача
			task.UserID = UserId;
			//встановлюємо часові данні
			task.CreatedAt = DateTime.Now;
			task.UpdatedAt = DateTime.Now;

			await _unitOfWork.Task.AddAsync(task);
			_logger.LogInformation("task created successfully");
			return Ok();
		}
		[HttpPut("{id}")]
		public async Task<ActionResult<Models.Task>> UpdateTask(int id, Models.Task updetedTask)
		{
			//перевіряємо чи збігаєься id із id завдання 
			if (id != updetedTask.Id)
			{
				return BadRequest();
			}
			//Встановлюємо час останнього оновлення
			updetedTask.UpdatedAt = DateTime.Now;
			//оновлюємо дані
			await _unitOfWork.Task.UpdateAsync(updetedTask);
			//change redirect
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Models.Task>> DeleteTask(int id) 
		{
			//отримуємо користувача
			var TaskToBeDeleted = await _unitOfWork.Task.GetFirstOrDefaultAsync(u => u.Id == id);
			//перевіряємо чи дане завдання існує
			if (TaskToBeDeleted == null) { return NotFound(); }

			//видаляємо в разі якщо завдання знайдено
			await _unitOfWork.Task.DeleteAsync(TaskToBeDeleted);
			_logger.LogInformation("task deleted successfully");
			return NoContent();
		}

	}
}
