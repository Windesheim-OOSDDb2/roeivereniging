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
        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.None;
        public User(int id, string firstName, string lastName, string emailAddress, string password) : base(id, $"{firstName} {lastName}")
        {
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}
