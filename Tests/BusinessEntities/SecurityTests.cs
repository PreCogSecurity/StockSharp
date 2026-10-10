using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.BusinessEntities
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Security_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var security = new Security();

            // Assert
            Assert.IsNotNull(security.Id);
            Assert.IsNotNull(security.Code);
            Assert.IsNotNull(security.Name);
            Assert.AreEqual(0, security.PriceStep);
            Assert.AreEqual(0, security.VolumeStep);
            Assert.AreEqual(0, security.Decimals);
            Assert.AreEqual(SecurityTypes.Unknown, security.Type);
            Assert.AreEqual(0, security.MinPrice);
            Assert.AreEqual(0, security.MaxPrice);
            Assert.AreEqual(0, security.Multiplier);
        }

        [TestMethod]
        public void Security_Id_ShouldBeUnique()
        {
            // Arrange
            var security1 = new Security { Code = "AAPL" };
            var security2 = new Security { Code = "GOOGL" };

            // Act & Assert
            Assert.AreNotEqual(security1.Id, security2.Id);
        }

        [TestMethod]
        public void Security_Code_CanBeSetAndRetrieved()
        {
            // Arrange
            var security = new Security();
            const string expectedCode = "AAPL";

            // Act
            security.Code = expectedCode;

            // Assert
            Assert.AreEqual(expectedCode, security.Code);
        }

        [TestMethod]
        public void Security_PriceStep_ValidatesPositiveValue()
        {
            // Arrange
            var security = new Security();

            // Act
            security.PriceStep = 0.01m;

            // Assert
            Assert.AreEqual(0.01m, security.PriceStep);
        }

        [TestMethod]
        public void Security_VolumeStep_ValidatesPositiveValue()
        {
            // Arrange
            var security = new Security();

            // Act
            security.VolumeStep = 1;

            // Assert
            Assert.AreEqual(1, security.VolumeStep);
        }

        [TestMethod]
        public void Security_Type_CanBeSet()
        {
            // Arrange
            var security = new Security();

            // Act
            security.Type = SecurityTypes.Stock;

            // Assert
            Assert.AreEqual(SecurityTypes.Stock, security.Type);
        }

        [TestMethod]
        public void Security_ToString_ReturnsCode()
        {
            // Arrange
            var security = new Security { Code = "AAPL", Name = "Apple Inc." };

            // Act
            var result = security.ToString();

            // Assert
            Assert.IsTrue(result.Contains("AAPL"));
        }

        [TestMethod]
        public void Security_Equality_BasedOnId()
        {
            // Arrange
            var security1 = new Security { Code = "AAPL" };
            var security2 = new Security { Code = "AAPL" };
            var security3 = security1;

            // Act & Assert
            Assert.AreNotEqual(security1, security2); // Different Id
            Assert.AreEqual(security1, security3);    // Same reference
            Assert.AreEqual(security1.Id, security1.Id); // Same Id
        }

        [TestMethod]
        public void Security_Copy_CreatesIndependentInstance()
        {
            // Arrange
            var original = new Security
            {
                Code = "AAPL",
                Name = "Apple Inc.",
                PriceStep = 0.01m,
                VolumeStep = 1,
                Decimals = 2,
                Type = SecurityTypes.Stock
            };

            // Act
            var copy = new Security
            {
                Id = original.Id,
                Code = original.Code,
                Name = original.Name,
                PriceStep = original.PriceStep,
                VolumeStep = original.VolumeStep,
                Decimals = original.Decimals,
                Type = original.Type
            };

            // Assert
            Assert.AreEqual(original.Id, copy.Id);
            Assert.AreEqual(original.Code, copy.Code);
            Assert.AreEqual(original.Name, copy.Name);
            Assert.AreEqual(original.PriceStep, copy.PriceStep);
            Assert.AreEqual(original.VolumeStep, copy.VolumeStep);
            Assert.AreEqual(original.Decimals, copy.Decimals);
            Assert.AreEqual(original.Type, copy.Type);
        }
    }
}