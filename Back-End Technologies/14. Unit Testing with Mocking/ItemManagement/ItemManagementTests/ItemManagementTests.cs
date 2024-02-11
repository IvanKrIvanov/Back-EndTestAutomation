using NUnit.Framework;
using Moq;
using ItemManagementApp.Services;
using ItemManagementLib.Repositories;
using ItemManagementLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace ItemManagement.Tests
{
    [TestFixture]
    public class ItemServiceTests
    {
        private ItemService _itemService;
        private Mock<IItemRepository> _mockItemRepository;
                

        [SetUp]
        public void Setup()
        {
            // Arrange: Create a mock instance of IItemRepository
            _mockItemRepository = new Mock<IItemRepository>();
            
            // Instantiate ItemService with the mocked repository
            _itemService = new ItemService(_mockItemRepository.Object);
        }


        [Test]
        public void AddItem_ShouldAddItem()
        {
            // Arrange
            var item = new Item { Name = "Test Item" };
            _mockItemRepository.Setup(x => x.AddItem(It.IsAny<Item>()));

            // Act
            _itemService.AddItem(item.Name);

            // Assert
            _mockItemRepository.Verify(x => x.AddItem(It.IsAny<Item>()), Times.Once());

        }

        [Test]
        public void AddItem_Shouldthrowerror_IfNameIsInvalid()
        {
            // Arrange
            string invalidName = "";
            _mockItemRepository.Setup(x => x.AddItem(It.IsAny<Item>())).Throws<ArgumentException>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemService.AddItem(invalidName));
            _mockItemRepository.Verify(x=>x.AddItem(It.IsAny<Item>()), Times.Once());

        }

        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Arrange
            var items = new List<Item>() { new Item { Id = 1, Name = "SampleItem", } };
            _mockItemRepository.Setup(x => x.GetAllItems()).Returns(items);

            // Act
            var result = _itemService.GetAllItems();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            _mockItemRepository.Verify(x => x.GetAllItems(), Times.Once());
            
        }

        [Test]
        public void GetItemByID_ShouldReturnItemByID_IfItemExist()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Single Item",};
            _mockItemRepository.Setup(x => x.GetItemById(item.Id)).Returns(item);

            // Act
            var result = _itemService.GetItemById(item.Id);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(item.Name));
            _mockItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once());

        }

        [Test]
        public void GetItemByID_ShouldReturNull_IfItemDoesNotExist()
        {
            // Arrange
            Item item = null;
            _mockItemRepository.Setup(x => x.GetItemById(It.IsAny<int>())).Returns(item);

            // Act
            var result = _itemService.GetItemById(123);

            // Assert
            Assert.Null(result);
            _mockItemRepository.Verify(x => x.GetItemById(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void UpdateItem_ShouldNotUpdateItemIfItemDoesNotExist()
        {
            // Arrange
            var nonExistingId = 1;
            _mockItemRepository.Setup(x => x.GetItemById(nonExistingId)).Returns<Item>(null);
            _mockItemRepository.Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act
            _itemService.UpdateItem(nonExistingId, "DoesNotMatter");

            // Assert
            _mockItemRepository.Verify(x => x.GetItemById(nonExistingId), Times.Once());
            _mockItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Never());
        }

        [Test]
        public void UpdateItem_ThrowExeption_IfItemNameIsInvalid()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Sample Item", };
            _mockItemRepository.Setup(x => x.GetItemById(item.Id)).Returns(item);
            _mockItemRepository.Setup(x => x.UpdateItem(It.IsAny<Item>())).Throws<ArgumentException>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemService.UpdateItem(item.Id, ""));
            _mockItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once());
            _mockItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once());
        }

        [Test]
        public void UpdateItem_ShouldUpdateItem_IfItemNameIsValid()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Sample Item", };
            _mockItemRepository.Setup(x => x.GetItemById(item.Id)).Returns(item);
            _mockItemRepository.Setup(x => x.UpdateItem(It.IsAny<Item>()));

            // Act 
            _itemService.UpdateItem(item.Id, "Sample Item UPDATED");

            // Assert
            _mockItemRepository.Verify(x => x.GetItemById(item.Id), Times.Once());
            _mockItemRepository.Verify(x => x.UpdateItem(It.IsAny<Item>()), Times.Once());
        }


        [Test]
        public void DeleteItem_ShouldCallDeleteItemOnRepository()
        {
            // Arrange
            var itemId = 12;
            _mockItemRepository.Setup(x => x.DeleteItem(itemId));

            // Act 
            _itemService.DeleteItem(itemId);

            // Assert
            _mockItemRepository.Verify(x => x.DeleteItem(itemId), Times.Once());
            
        }

        [TestCase ("", false)]
        [TestCase (null, false)]
        [TestCase ("aaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
        [TestCase ("a", true)]
        [TestCase ("sample", true)]
        [TestCase ("samplename", true)]
        public void ValidateItemName_WhenNameIsValid_ShouldReturnTrue(string name, bool isValid)
        {
            // Arrange
            //var itemName = "SampleName";

            // Act
            var result = _itemService.ValidateItemName(name);

            // Assert
            Assert.That(result, Is.EqualTo(isValid));
        }

    }
}