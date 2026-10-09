using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.BusinessEntities
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void Portfolio_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var portfolio = new Portfolio();

            // Assert
            Assert.IsNotNull(portfolio.Name);
            Assert.AreEqual(0, portfolio.BeginValue);
            Assert.AreEqual(0, portfolio.CurrentValue);
            Assert.AreEqual(0, portfolio.BlockedValue);
            Assert.AreEqual(0, portfolio.CommissionValue);
        }

        [TestMethod]
        public void Portfolio_Name_CanBeSet()
        {
            // Arrange
            var portfolio = new Portfolio();
            const string name = "TestPortfolio";

            // Act
            portfolio.Name = name;

            // Assert
            Assert.AreEqual(name, portfolio.Name);
        }

        [TestMethod]
        public void Portfolio_BeginValue_SetsInitialCapital()
        {
            // Arrange
            var portfolio = new Portfolio();
            const decimal initialCapital = 100000m;

            // Act
            portfolio.BeginValue = initialCapital;

            // Assert
            Assert.AreEqual(initialCapital, portfolio.BeginValue);
        }

        [TestMethod]
        public void Portfolio_CurrentValue_TracksCurrentEquity()
        {
            // Arrange
            var portfolio = new Portfolio { BeginValue = 100000m };

            // Act
            portfolio.CurrentValue = 105000m; // 5% gain

            // Assert
            Assert.AreEqual(105000m, portfolio.CurrentValue);
        }

        [TestMethod]
        public void Portfolio_BlockedValue_ReservesMargin()
        {
            // Arrange
            var portfolio = new Portfolio { CurrentValue = 100000m };

            // Act
            portfolio.BlockedValue = 20000m; // Margin reserved

            // Assert
            Assert.AreEqual(20000m, portfolio.BlockedValue);
        }

        [TestMethod]
        public void Portfolio_FreeValue_CalculatedCorrectly()
        {
            // Arrange
            var portfolio = new Portfolio
            {
                CurrentValue = 100000m,
                BlockedValue = 20000m
            };

            // Act
            var freeValue = portfolio.CurrentValue - portfolio.BlockedValue;

            // Assert
            Assert.AreEqual(80000m, freeValue);
        }

        [TestMethod]
        public void Portfolio_CommissionValue_TracksFees()
        {
            // Arrange
            var portfolio = new Portfolio();

            // Act
            portfolio.CommissionValue = 150.75m;

            // Assert
            Assert.AreEqual(150.75m, portfolio.CommissionValue);
        }
    }
}