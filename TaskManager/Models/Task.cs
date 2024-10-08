﻿using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
	public class Task
	{
		[Key]
		public int Id { get; set; }
        [Required]
        public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }

        [Required]
        public int UserID { get; set; }
		[ForeignKey("UserID")]
		public User user { get; set; }

        [Required]
        public string Status { get; set; }
		[Required]
		public string Priority { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public DateTime UpdatedAt { get; set; }
    }
}
