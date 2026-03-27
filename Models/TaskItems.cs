using ServiceStack.DataAnnotations;

namespace TaskManagement.Models
{
    /// <summary>
    /// Model Class of TaskItem
    /// </summary>
    public class TaskItems
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Task Title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Task Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Task Status (A - Active/ P - Pending/ C - Completed)
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Task Priority
        /// </summary>
        public string? Priority { get; set; }

        /// <summary>
        /// Task Created DateTime
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// LoggedIn User ID
        /// </summary>
        public int UserId { get; set; } = 0;
    }

    public class TaskAssign
    {
        public List<int> taskIds { get; set; }
        public int userId { get; set; }
    }
}
