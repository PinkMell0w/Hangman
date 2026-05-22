using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class RegisterRequestDto
    {

        [DataMember] public int RoleId { get; set; }
        [DataMember] public string FullName { get; set; }
        [DataMember] public string DateOfBirth { get; set; }
        [DataMember] public string PhoneNumber { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public string Email { get; set; }
        [DataMember] public string Password { get; set; }
    }
}
