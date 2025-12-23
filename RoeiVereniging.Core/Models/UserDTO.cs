namespace RoeiVereniging.Core.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int DateofBirth { get; set; }

        public int Level { get; set; }

        public int ReservationsCount { get; set; }

        public int DamageCount { get; set; }

        public int RegistrationDate { get; set; }

        public int LastActiveDate { get; set; }

        public UserDTO(int userId, string name)
        {
            UserId = userId;
            Name = name;
            //LastName = lastName;
            //EmailAddress = emailAddress;
            //DateofBirth = dateofBirth;
            //Level = level;
            //ReservationsCount = reservationsCount;
            //DamageCount = damageCount;
            //RegistrationDate = registrationDate;
            //LastActiveDate = lastActiveDate;
        }

    }
}
