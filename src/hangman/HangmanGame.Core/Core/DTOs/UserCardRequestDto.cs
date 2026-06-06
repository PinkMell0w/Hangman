using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class UserCardRequestDto
    {
        [DataMember] public string Username { get; set; }
        [DataMember] public int RequesterId { get; set; }
    }
}
