using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "TaskAssigned";

        public bool IsRead { get; set; } = false;

        public int? RelatedTaskId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }
    }
}
