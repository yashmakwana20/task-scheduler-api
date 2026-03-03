namespace TaskManagement.DTOs
{
    /// <summary>
    /// DTO Model For Register User
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// User's Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// User's Password
        /// </summary>
        public string? Password { get; set; }
    }

}
