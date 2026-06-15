using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.Domain
{
    public class UserCard
    {
        [DataMember] public int UserId { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public string AvatarUrl { get; set; }
        [DataMember] public int TotalScore { get; set; }
        [DataMember] public double WinRate { get; set; }
}
}
