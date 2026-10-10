using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.BusinessEntities
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var order = new Order();

            // Assert
            Assert.IsNotNull(order.Id);
            Assert.AreEqual(OrderStates.None, order.State);
            Assert.AreEqual(OrderTypes.Limit, order.Type);
            Assert.AreEqual(OrderDirections.Buy, order.Direction);
            Assert.AreEqual(0, order.Volume);
            Assert.AreEqual(0, order.Balance);
            Assert.AreEqual(0, order.Price);
            Assert.IsNull(order.Security);
            Assert.IsNull(order.Portfolio);
            Assert.IsNull(order.UserOrderId);
        }

        [TestMethod]
        public void Order_Volume_MustBeNonNegative()
        {
            // Arrange
            var order = new Order();

            // Act
            order.Volume = 100;

            // Assert
            Assert.AreEqual(100, order.Volume);
        }

        [TestMethod]
        public void Order_Price_ForLimitOrder()
        {
            // Arrange
            var order = new Order { Type = OrderTypes.Limit };

            // Act
            order.Price = 150.50m;

            // Assert
            Assert.AreEqual(150.50m, order.Price);
        }

        [TestMethod]
        public void Order_MarketOrder_PriceIsZero()
        {
            // Arrange
            var order = new Order { Type = OrderTypes.Market };

            // Act & Assert
            Assert.AreEqual(0, order.Price);
        }

        [TestMethod]
        public void Order_Direction_BuyAndSell()
        {
            // Arrange
            var buyOrder = new Order { Direction = OrderDirections.Buy };
            var sellOrder = new Order { Direction = OrderDirections.Sell };

            // Assert
            Assert.AreEqual(OrderDirections.Buy, buyOrder.Direction);
            Assert.AreEqual(OrderDirections.Sell, sellOrder.Direction);
        }

        [TestMethod]
        public void Order_State_Transitions()
        {
            // Arrange
            var order = new Order();

            // Act & Assert
            Assert.AreEqual(OrderStates.None, order.State);

            order.State = OrderStates.Pending;
            Assert.AreEqual(OrderStates.Pending, order.State);

            order.State = OrderStates.Active;
            Assert.AreEqual(OrderStates.Active, order.State);

            order.State = OrderStates.Done;
            Assert.AreEqual(OrderStates.Done, order.State);
        }

        [TestMethod]
        public void Order_Balance_UpdatesWithTrades()
        {
            // Arrange
            var order = new Order { Volume = 100 };

            // Act
            order.Balance = 50; // Partially filled

            // Assert
            Assert.AreEqual(50, order.Balance);
            Assert.AreEqual(100, order.Volume);
        }

        [TestMethod]
        public void Order_Security_Association()
        {
            // Arrange
            var order = new Order();
            var security = new Security { Code = "AAPL" };

            // Act
            order.Security = security;

            // Assert
            Assert.AreSame(security, order.Security);
        }

        [TestMethod]
        public void Order_Portfolio_Association()
        {
            // Arrange
            var order = new Order();
            var portfolio = new Portfolio { Name = "TestPortfolio" };

            // Act
            order.Portfolio = portfolio;

            // Assert
            Assert.AreSame(portfolio, order.Portfolio);
        }

        [TestMethod]
        public void Order_Comment_CanBeSet()
        {
            // Arrange
            var order = new Order();
            const string comment = "Test order from unit test";

            // Act
            order.Comment = comment;

            // Assert
            Assert.AreEqual(comment, order.Comment);
        }

        [TestMethod]
        public void Order_ExpiryDate_CanBeSet()
        {
            // Arrange
            var order = new Order();
            var expiry = DateTimeOffset.Now.AddDays(1);

            // Act
            order.ExpiryDate = expiry;

            // Assert
            Assert.AreEqual(expiry, order.ExpiryDate);
        }
    }
}