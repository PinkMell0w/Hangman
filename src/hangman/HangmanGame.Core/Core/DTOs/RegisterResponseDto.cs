using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class RegisterResponseDto
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public int UserId { get; set; }
    }
}
