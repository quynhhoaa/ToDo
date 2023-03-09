using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }
        [MaxLength(100)]
        [Required]
        public string Username { get; set; }
        [MaxLength(200)]
        [Required]
        public string PasswordHash { get; set; }
        public ICollection<Todo> Tasks { get; set; }
    }
}
