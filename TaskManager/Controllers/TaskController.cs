using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
