using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
	public class ApplicationDbContext:  DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = 1,
					Username = "admin",
					Email = "admin@gmail.com",
					PasswordHash = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAkb64s18hYUCWIzdN904QWQAAAAACAAAAAAAQZgAAAAEAACAAAAC8BqtfKq0oKZXOMmpOl9zvnhRpXCPkjgiuADALF7cPaAAAAAAOgAAAAAIAACAAAAAeqoVyRo/VH58lJwtphsCe/yuD48lWVbh8XKZ1Gm5jNBAAAAAxsSxUte/Rcl4Qg++FLxnhQAAAANcvVZSHD5MWdtuEGJwY8wQiFy5XO+pU2cnL7rOxjE4RjD8Jb4CKKqjE7RCjd2hCPobJpS2WKLRUGMU0D12ra/Q=",
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now
				}
				
			);
		}
	}
}
