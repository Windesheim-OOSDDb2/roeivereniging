using Moq;
using NUnit.Framework;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;

namespace TestCore
{
    public interface IReservationRepository
    {
        void Save(Reservation reservation);
        Reservation GetMostRecent();
    }

    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public void AddReservation(Reservation reservation)
        {
            _repository.Save(reservation);
        }

        public Reservation GetLatestReservation()
        {
            return _repository.GetMostRecent();
        }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SaveReservation_ShouldStoreAndReturnMostRecentReservation()
        {
            // Arrange
            var mockRepo = new Mock<IReservationRepository>();
            var reservation = new Reservation { Id = 1, Name = "Test Reservation" };

            mockRepo.Setup(r => r.Save(It.IsAny<Reservation>())).Verifiable();
            mockRepo.Setup(r => r.GetMostRecent()).Returns(reservation);

            var service = new ReservationService(mockRepo.Object);

            // Act
            service.AddReservation(reservation);
            var latest = service.GetLatestReservation();

            // Assert
            mockRepo.Verify(r => r.Save(reservation), Times.Once);
            Assert.AreEqual(reservation, latest);
        }

        [Test]
        public void EditUser_ShouldUpdateUser_WhenValidInput()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var userId = 1;
            var originalUser = new User(
                userId, "John", "Doe", "john@example.com", "hashedpassword", Role.User, BoatLevel.Beginner,
                DateOnly.FromDateTime(DateTime.Parse("2000-01-01")), DateTime.Now.AddYears(-1), DateTime.Now.AddDays(-1)
            );
            var updatedUser = new User(
                userId, "Jane", "Smith", "jane@example.com", "newhashedpassword", Role.Admin, BoatLevel.Alles,
                DateOnly.FromDateTime(DateTime.Parse("1995-05-05")), originalUser.RegistrationDate, DateTime.Now
            );

            mockRepo.Setup(r => r.Get(userId)).Returns(originalUser);
            mockRepo.Setup(r => r.Update(It.IsAny<User>())).Returns(updatedUser);

            var userService = new RoeiVereniging.Core.Services.UserService(mockRepo.Object);

            // Act
            var result = userService.Update(updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.FirstName, result.FirstName);
            Assert.AreEqual(updatedUser.LastName, result.LastName);
            Assert.AreEqual(updatedUser.EmailAddress, result.EmailAddress);
            Assert.AreEqual(updatedUser.Role, result.Role);
            mockRepo.Verify(r => r.Update(It.Is<User>(u => u.UserId == userId)), Times.Once);
        }
    }
}