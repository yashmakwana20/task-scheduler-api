using ServiceStack.DataAnnotations;

namespace TaskManagement.Models
{
    public class RefreshToken
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Authorization Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User Table Object
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Token Expiry Date
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Flag for token revokation
        /// </summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>
        /// Token generation datetime
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
