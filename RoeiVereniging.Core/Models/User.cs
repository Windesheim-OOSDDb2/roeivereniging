using CommunityToolkit.Mvvm.ComponentModel;
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
        public int UserId { get; set; }

        public string FirstName;

        public string LastName;

        public DateTime RegistrationDate { get; set; }

        public DateTime LastActiveDate { get; set; }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public BoatLevel Level { get; set; } = BoatLevel.Beginner;
        public DateOnly DateOfBirth { get; set; }
        public User(int id, string firstName, string lastName, string emailAddress, string password, Role role, BoatLevel level, DateOnly dateofBirth, DateTime registrationDate) : base(id, $"{firstName} {lastName}")
        {
            UserId = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            Role = role;
            Level = level;
            DateOfBirth = dateofBirth;
            RegistrationDate = registrationDate;
            //LastActiveDate = lastActiveDate;
        }
    }
}
