using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
