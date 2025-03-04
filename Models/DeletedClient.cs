using System.ComponentModel.DataAnnotations;

namespace EnterApp.Models
{
    public class DeletedClient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public  string Unique_Id { get; set; }

        [Required]
        public  string Name { get; set; }

        [Required]
        public  string Role { get; set; }
        [Required]
        public DateTime Deleted_At { get; set; }
    }
}
