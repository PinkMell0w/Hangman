using HangmanGame.Core.Core.Domain;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class UserCardResponseDto
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public List<UserCard> Users { get; set; }
    }
}
