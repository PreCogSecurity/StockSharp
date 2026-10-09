using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Slippage;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class SlippageManagerTests
    {
        [TestMethod]
        public void SlippageManager_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var slippageManager = new SlippageManager();

            // Assert
            Assert.IsNotNull(slippageManager.Model);
        }

        [TestMethod]
        public void FixedSlippageModel_CalculatesCorrectly()
        {
            // Arrange
            var model = new FixedSlippageModel { Slippage = 0.01m }; // 1 cent fixed
            var order = new Order { Price = 150m, Direction = OrderDirections.Buy };
            var trade = new Trade { Price = 150m };

            // Act
            var slippage = model.Calculate(order, trade);

            // Assert
            Assert.AreEqual(0.01m, slippage);
        }

        [TestMethod]
        public void PercentSlippageModel_CalculatesCorrectly()
        {
            // Arrange
            var model = new PercentSlippageModel { Slippage = 0.001m }; // 0.1%
            var order = new Order { Price = 150m, Direction = OrderDirections.Buy };
            var trade = new Trade { Price = 150m };

            // Act
            var slippage = model.Calculate(order, trade);

            // Assert
            Assert.AreEqual(0.15m, slippage); // 150 * 0.001
        }

        [TestMethod]
        public void VolumeSlippageModel_CalculatesCorrectly()
        {
            // Arrange
            var model = new VolumeSlippageModel 
            { 
                Slippage = 0.0001m, // 0.01% per unit volume
                Volume = 1000 // Reference volume
            };
            var order = new Order { Volume = 5000, Price = 150m, Direction = OrderDirections.Buy };
            var trade = new Trade { Volume = 5000, Price = 150m };

            // Act
            var slippage = model.Calculate(order, trade);

            // Assert
            // Slippage = 0.0001 * (5000/1000) * 150 = 0.0001 * 5 * 150 = 0.075
            Assert.AreEqual(0.075m, slippage);
        }

        [TestMethod]
        public void Slippage_BuyOrder_IncreasesPrice()
        {
            // Arrange
            var model = new FixedSlippageModel { Slippage = 0.01m };
            var order = new Order { Price = 150m, Direction = OrderDirections.Buy };
            var trade = new Trade { Price = 150m };

            // Act
            var slippage = model.Calculate(order, trade);
            var executedPrice = trade.Price + slippage;

            // Assert
            Assert.AreEqual(150.01m, executedPrice);
        }

        [TestMethod]
        public void Slippage_SellOrder_DecreasesPrice()
        {
            // Arrange
            var model = new FixedSlippageModel { Slippage = 0.01m };
            var order = new Order { Price = 150m, Direction = OrderDirections.Sell };
            var trade = new Trade { Price = 150m };

            // Act
            var slippage = model.Calculate(order, trade);
            var executedPrice = trade.Price - slippage;

            // Assert
            Assert.AreEqual(149.99m, executedPrice);
        }

        [TestMethod]
        public void SlippageManager_WithModel_CalculatesCorrectly()
        {
            // Arrange
            var manager = new SlippageManager
            {
                Model = new FixedSlippageModel { Slippage = 0.02m }
            };
            var order = new Order { Price = 100m, Direction = OrderDirections.Buy };
            var trade = new Trade { Price = 100m };

            // Act
            var slippage = manager.Calculate(order, trade);

            // Assert
            Assert.AreEqual(0.02m, slippage);
        }
    }
}