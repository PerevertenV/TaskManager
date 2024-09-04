using TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.Repository.IRepository;

namespace TaskManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyCors", builder =>
				{
					builder.WithOrigins("http://localhost:4200")
					.AllowAnyMethod()
					.AllowAnyHeader();
				});
			});

			builder.Services.AddScoped<IUnitOfWork, IUnitOfWork>();

			builder.Services.AddControllers();

			var app = builder.Build();

			app.MapGet("/", () => "Hello World!");

			app.UseCors("MyCors");

			app.MapControllers();

			app.Run();
		}
	}
}
