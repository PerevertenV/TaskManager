using TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.Repository.IRepository;
using TaskManager.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Services;

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

			builder.Services.AddScoped<JwtTokenService>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
			});

			// Додати службу авторизації
			builder.Services.AddAuthorization();

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddControllers();

			builder.Logging.ClearProviders();

			builder.Logging.AddConsole(); 

			builder.Logging.AddDebug();

			var app = builder.Build();

			app.MapGet("/", () => "Hello World!");

			app.UseCors("MyCors");

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
