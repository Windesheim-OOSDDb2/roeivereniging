using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
    public partial class User : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public BoatLevel Level { get; set; } = BoatLevel.Beginner;
        public DateOnly DateOfBirth { get; set; }
        public User(int id, string name, string emailAddress, string password, Role role, BoatLevel level, DateOnly dateOfBirth) : base(id, name)
        {
            EmailAddress = emailAddress;
            Password = password;
            Role = role;
            Level = level;
            DateOfBirth = dateOfBirth;
        }
    }
}
