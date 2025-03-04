using System;
using System.ComponentModel.DataAnnotations;

namespace EnterApp.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string UserName { get; set; }
        [Required]
        public  string UserRole { get; set; }
        [Required]
        public DateTime ActionTime { get; set; }
        [Required]
        public  string Action { get; set; }
        [Required]
        public bool IsError { get; set; }
    }
}
