using System;

namespace HangmanGame.Core.Core.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string pwdHash { get; set; }
        public string Salt { get; set; }
        public int isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
