using ServiceStack.DataAnnotations;

namespace TaskManagement.Models
{
    /// <summary>
    /// Model Of User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// User's Password
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// User Role
        /// </summary>
        public string? UserRole { get; set; }
    }
}
