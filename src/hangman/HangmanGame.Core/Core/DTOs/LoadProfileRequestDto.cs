using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class LoadProfileRequestDto
    {
        [DataMember]
        public int UserId { get; set; }
    }
}
