using GardenConsoleAPI.Business;
using GardenConsoleAPI.Business.Contracts;
using GardenConsoleAPI.Data.Models;
using GardenConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace GardenConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestPlantsDbContext dbContext;
        private IPlantsManager plantsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestPlantsDbContext();
            this.plantsManager = new PlantsManager(new PlantsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddPlantAsync_ShouldAddNewPlant()
        {
            // Arrange
            var plantToAdd = new Plant
            {
                CatalogNumber = "ABCD12345678", 
                Name = "Test Plant",
                PlantType = "Test Type",
                FoodType = "Test Food",
                Quantity = 10
            };

            // Act
            await plantsManager.AddAsync(plantToAdd);
            var addedPlant = await plantsManager.GetSpecificAsync(plantToAdd.CatalogNumber);

            // Assert
            Assert.IsNotNull(addedPlant);
            Assert.That(addedPlant.Name, Is.EqualTo(plantToAdd.Name));
            Assert.That(addedPlant.PlantType, Is.EqualTo(plantToAdd.PlantType));
            Assert.That(addedPlant.FoodType, Is.EqualTo(plantToAdd.FoodType));
            Assert.That(addedPlant.Quantity, Is.EqualTo(plantToAdd.Quantity));
            Assert.That(addedPlant.CatalogNumber, Is.EqualTo(plantToAdd.CatalogNumber));
        }

        //Negative test
        [Test]
        public async Task AddPlantAsync_TryToAddPlantWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidPlant = new Plant
            {
                Name = "Invalid Plant",
                PlantType = "Invalid Plant Type",
                FoodType = "Invalid Food Type",
                Quantity = -5, 
                CatalogNumber = "InvalidCatalogNumber", 
                IsEdible = false 
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () => await plantsManager.AddAsync(invalidPlant));

        }

        [Test]
        public async Task DeletePlantAsync_WithValidCatalogNumber_ShouldRemovePlantFromDb()
        {
            // Arrange

            var plantToAdd = new Plant
            {
                CatalogNumber = "01HP01PRFC6E", 
                Name = "Test Plant",
                PlantType = "Test Plant Type",
                FoodType = "Test Food Type",
                Quantity = 10,
                IsEdible = true
            };
            await plantsManager.AddAsync(plantToAdd);

            // Act

            await plantsManager.DeleteAsync("01HP01PRFC6E");

            // Assert

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await plantsManager.GetSpecificAsync("01HP01PRFC6E");
            });
        }

        [Test]
        public async Task DeletePlantAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException()
        {
            // Arrange
            string nullOrWhiteSpaceCatalogNumber = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await plantsManager.DeleteAsync(nullOrWhiteSpaceCatalogNumber));
        }

        [Test]
        public async Task GetAllAsync_WhenPlantsExist_ShouldReturnAllPlants()
        {
            // Arrange
            var plant1 = new Plant
            {
                CatalogNumber = "123456789012",
                Name = "Plant 1",
                PlantType = "Type 1",
                FoodType = "Food 1",
                Quantity = 10,
                IsEdible = true
            };

            var plant2 = new Plant
            {
                CatalogNumber = "234567890123",
                Name = "Plant 2",
                PlantType = "Type 2",
                FoodType = "Food 2",
                Quantity = 15,
                IsEdible = false
            };

            await plantsManager.AddAsync(plant1);
            await plantsManager.AddAsync(plant2);

            // Act
            var plants = await plantsManager.GetAllAsync();

            // Assert
            Assert.IsNotNull(plants);
            Assert.That(plants.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_WhenNoPlantsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            // Act
            async Task Act() => await plantsManager.GetAllAsync();

            // Assert
            Assert.ThrowsAsync<KeyNotFoundException>(Act);
        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithExistingFoodType_ShouldReturnMatchingPlants()
        {
            // Arrange
            var foodType = "Vegetable";
            var plant1 = new Plant
            {
                CatalogNumber = "123456789012",
                Name = "Carrot",
                PlantType = "Root Vegetable",
                FoodType = foodType,
                Quantity = 10,
                IsEdible = true
            };

            var plant2 = new Plant
            {
                CatalogNumber = "234567890123",
                Name = "Broccoli",
                PlantType = "Cruciferous Vegetable",
                FoodType = foodType,
                Quantity = 15,
                IsEdible = true
            };

            await plantsManager.AddAsync(plant1);
            await plantsManager.AddAsync(plant2);

            // Act
            var plants = await plantsManager.SearchByFoodTypeAsync(foodType);

            // Assert
            Assert.IsNotNull(plants);
            Assert.That(plants.Count(), Is.EqualTo(2));

        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithNonExistingFoodType_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var nonExistingFoodType = "NonExistingFoodType";

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await plantsManager.SearchByFoodTypeAsync(nonExistingFoodType);
            });

        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnPlant()
        {
            // Arrange
            var plantToAdd = new Plant
            {
                CatalogNumber = "ABC123456789",
                Name = "SamplePlant",
                FoodType = "SampleFood",
                PlantType = "SampleType",
                Quantity = 10,
                IsEdible = true
            };
            await plantsManager.AddAsync(plantToAdd);

            // Act
            var result = await plantsManager.GetSpecificAsync(plantToAdd.CatalogNumber);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.CatalogNumber, Is.EqualTo(plantToAdd.CatalogNumber));
            Assert.That(result.Name, Is.EqualTo(plantToAdd.Name));
            Assert.That(result.FoodType, Is.EqualTo(plantToAdd.FoodType));
            Assert.That(result.PlantType, Is.EqualTo(plantToAdd.PlantType));
            Assert.That(result.Quantity, Is.EqualTo(plantToAdd.Quantity));
            Assert.That(result.IsEdible, Is.EqualTo(plantToAdd.IsEdible));

        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            string invalidCatalogNumber = "InvalidCatalogNumber";

            // Act
            async Task Act() => await plantsManager.GetSpecificAsync(invalidCatalogNumber);

            // Assert
            Assert.ThrowsAsync<KeyNotFoundException>(Act);

        }

        [Test]
        public async Task UpdateAsync_WithValidPlant_ShouldUpdatePlant()
        {
            // Arrange
            var originalPlant = new Plant
            {
                CatalogNumber = "123456789012", 
                PlantType = "Tree", 
                FoodType = "Fruit", 
                Name = "OriginalName",
          
            };

            await plantsManager.AddAsync(originalPlant); 

            var updatedName = "UpdatedName";

            // Act
            originalPlant.Name = updatedName;
            await plantsManager.UpdateAsync(originalPlant);

            // Assert
            var updatedPlant = await plantsManager.GetSpecificAsync(originalPlant.CatalogNumber);
            Assert.IsNotNull(updatedPlant); 
            Assert.That(updatedPlant.Name, Is.EqualTo(updatedName));

        }

        [Test]
        public async Task UpdateAsync_WithInvalidPlant_ShouldThrowValidationException()
        {
            // Arrange
            var invalidPlant = new Plant
            {
                CatalogNumber = "123",
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await plantsManager.UpdateAsync(invalidPlant);
            });

        }
    }
}
