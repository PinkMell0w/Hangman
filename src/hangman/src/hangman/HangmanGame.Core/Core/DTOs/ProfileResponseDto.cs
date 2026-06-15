using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class ProfileResponseDto
    {
        [DataMember] public int UserId { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public string FullName { get; set; }
        [DataMember] public int GamesPlayed { get; set; }
        [DataMember] public int GamesWon { get; set; }
        [DataMember] public int TotalScore { get; set; }
        [DataMember] public double WinRate { get; set; }
        [DataMember] public string AvatarUrl { get; set; }
        [DataMember] public string Theme { get; set; }
        [DataMember] public string Bio { get; set; }
    }
}