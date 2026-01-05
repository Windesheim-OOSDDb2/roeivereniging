namespace RoeiVereniging.Core.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int DateofBirth { get; set; }

        public int Level { get; set; }

        public int ReservationsCount { get; set; }

        public int DamageCount { get; set; }

        public string RegistrationDate { get; set; }

        public string LastActiveDate { get; set; }

        public UserDTO(int userId, string firstName, string lastName, string registrationDate, string lastActiveDate)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            RegistrationDate = registrationDate;
            LastActiveDate = lastActiveDate;
        }

    }
}
