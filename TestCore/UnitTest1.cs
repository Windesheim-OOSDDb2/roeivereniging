using Moq;
using NUnit.Framework;

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
        public void ExampleTest()
        {
            Assert.Pass();
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
    }
}