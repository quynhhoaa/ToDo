using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Models
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Categpry")]
        public int CategoryId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}
