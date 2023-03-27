using System.ComponentModel.DataAnnotations;

namespace TodoList.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Usename { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
