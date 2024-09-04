using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Required]
        public string Username { get; set; }
		[Required]
        public string Email { get; set; }
		[Required]
		public string PasswordHash { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
    }
}
