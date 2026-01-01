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

        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        public DateTime RegistrationDate { get; set; }

        [ObservableProperty]
        private DateTime lastActiveDate;

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.None;
        public User(int id, string firstName, string lastName, string emailAddress, string password, DateTime registrationDate, DateTime lastActiveDate) : base(id, $"{firstName} {lastName}")
        {
            UserId = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            RegistrationDate = registrationDate;
            LastActiveDate = lastActiveDate;
        }
    }
}
