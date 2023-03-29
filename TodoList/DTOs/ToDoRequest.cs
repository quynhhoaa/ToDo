using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.DTOs
{
    public class ToDoRequest
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
