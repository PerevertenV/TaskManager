using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Repository.IRepository;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        public TaskController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasksAsync()
		{
			//отримуємо та одразу ж повертаємо елменти
			return Ok(await _unitOfWork.Task.GetAllAsync());
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
		public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task) 
		{
			return Ok();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Models.Task>> UpdateTask(int id)
		{
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
			return NoContent();
		}

	}
}
