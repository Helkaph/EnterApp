using System.ComponentModel.DataAnnotations;

namespace EnterApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public  string Unique_Id { get; set; }

        [Required]
        public  string Name { get; set; }

        [Required]
        public bool Authorized { get; set; }

        [Required]
        public  string Role { get; set; }
    }
}
