using GetGreeting;
using Moq;

namespace Get.Greeting.Test
{
    public class GreetingProviderTest

    {
        private GreetingProvider _greetingProvider;
        private Mock<ITimeProvider> _mockedTimeProvider;

        [SetUp]
        public void SetUp()
        {
            _mockedTimeProvider = new Mock<ITimeProvider>();
            _greetingProvider = new GreetingProvider(_mockedTimeProvider.Object);
        }
        [Test]
        public void GetGreeting_ShouldReturnAMorningMessage_WhenItIsMorning()
        {
            //Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 9, 9, 9));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good morning!"));
        }

        [Test]
        public void GetGreeting_ShouldReturnAAfternoonMessage_WhenItIsAfternoon()
        {   
            //Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 13, 13, 9));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good afternoon!"));
        }
        [Test]
        public void GetGreeting_ShouldReturnAeveningMessage_WhenItIsEvening()
        {
            //Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 20, 13, 9));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good evening!"));
        }

        [Test]
        public void GetGreeting_ShouldReturnANightMessage_WhenItIsNight()
        {
            //Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, 23, 13, 9));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo("Good night!"));
        }

        [TestCase("Good night!", 4)]
        [TestCase("Good evening!", 19)]
        [TestCase("Good afternoon!", 13)]
        [TestCase("Good morning!", 11)]
        public void GetGreeting_ShouldReturnCorrectMessage_whenTimeIsCorrect(string expectedMessage, int currentHour)
        {
            //Arrange
            _mockedTimeProvider.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2000, 2, 2, currentHour, 13, 9));

            // Act
            var result = _greetingProvider.GetGreeting();

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }
    }
}