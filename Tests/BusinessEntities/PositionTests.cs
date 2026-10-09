using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.BusinessEntities
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Position_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var position = new Position();

            // Assert
            Assert.AreEqual(0, position.CurrentValue);
            Assert.AreEqual(0, position.BlockedValue);
            Assert.AreEqual(0, position.AveragePrice);
            Assert.IsNull(position.Security);
            Assert.IsNull(position.Portfolio);
            Assert.IsNull(position.Strategy);
        }

        [TestMethod]
        public void Position_CurrentValue_TracksNetPosition()
        {
            // Arrange
            var position = new Position();

            // Act
            position.CurrentValue = 100; // Long 100 shares

            // Assert
            Assert.AreEqual(100, position.CurrentValue);
        }

        [TestMethod]
        public void Position_ShortPosition_NegativeValue()
        {
            // Arrange
            var position = new Position();

            // Act
            position.CurrentValue = -50; // Short 50 shares

            // Assert
            Assert.AreEqual(-50, position.CurrentValue);
        }

        [TestMethod]
        public void Position_BlockedValue_ReservesForOrders()
        {
            // Arrange
            var position = new Position { CurrentValue = 100 };

            // Act
            position.BlockedValue = 30; // 30 shares reserved for pending orders

            // Assert
            Assert.AreEqual(30, position.BlockedValue);
        }

        [TestMethod]
        public void Position_AveragePrice_CalculatesCostBasis()
        {
            // Arrange
            var position = new Position();

            // Act
            position.AveragePrice = 150.25m;

            // Assert
            Assert.AreEqual(150.25m, position.AveragePrice);
        }

        [TestMethod]
        public void Position_Security_Association()
        {
            // Arrange
            var position = new Position();
            var security = new Security { Code = "AAPL" };

            // Act
            position.Security = security;

            // Assert
            Assert.AreSame(security, position.Security);
        }

        [TestMethod]
        public void Position_Portfolio_Association()
        {
            // Arrange
            var position = new Position();
            var portfolio = new Portfolio { Name = "TestPortfolio" };

            // Act
            position.Portfolio = portfolio;

            // Assert
            Assert.AreSame(portfolio, position.Portfolio);
        }

        [TestMethod]
        public void Position_UnrealizedPnL_CalculatedCorrectly()
        {
            // Arrange
            var position = new Position
            {
                CurrentValue = 100,
                AveragePrice = 150m,
                Security = new Security { Code = "AAPL" }
            };
            var currentPrice = 155m;

            // Act
            var unrealizedPnL = position.CurrentValue * (currentPrice - position.AveragePrice);

            // Assert
            Assert.AreEqual(500m, unrealizedPnL); // 100 * (155 - 150) = 500
        }

        [TestMethod]
        public void Position_RealizedPnL_FromClosedTrades()
        {
            // Arrange
            var position = new Position();

            // Act
            position.RealizedPnL = 2500m;

            // Assert
            Assert.AreEqual(2500m, position.RealizedPnL);
        }
    }
}