using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Messages;

namespace StockSharp.Tests.Messages
{
    [TestClass]
    public class QuoteChangeMessageTests
    {
        [TestMethod]
        public void QuoteChangeMessage_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var message = new QuoteChangeMessage();

            // Assert
            Assert.AreEqual(MessageTypes.QuoteChange, message.Type);
            Assert.IsNotNull(message.Bids);
            Assert.IsNotNull(message.Asks);
            Assert.AreEqual(0, message.Bids.Count);
            Assert.AreEqual(0, message.Asks.Count);
        }

        [TestMethod]
        public void QuoteChangeMessage_AddBid_CanAddBidLevels()
        {
            // Arrange
            var message = new QuoteChangeMessage();

            // Act
            message.Bids.Add(new QuoteChange { Price = 150.00m, Volume = 100 });
            message.Bids.Add(new QuoteChange { Price = 149.99m, Volume = 200 });

            // Assert
            Assert.AreEqual(2, message.Bids.Count);
            Assert.AreEqual(150.00m, message.Bids[0].Price);
            Assert.AreEqual(100, message.Bids[0].Volume);
            Assert.AreEqual(149.99m, message.Bids[1].Price);
            Assert.AreEqual(200, message.Bids[1].Volume);
        }

        [TestMethod]
        public void QuoteChangeMessage_AddAsk_CanAddAskLevels()
        {
            // Arrange
            var message = new QuoteChangeMessage();

            // Act
            message.Asks.Add(new QuoteChange { Price = 150.01m, Volume = 150 });
            message.Asks.Add(new QuoteChange { Price = 150.02m, Volume = 250 });

            // Assert
            Assert.AreEqual(2, message.Asks.Count);
            Assert.AreEqual(150.01m, message.Asks[0].Price);
            Assert.AreEqual(150, message.Asks[0].Volume);
            Assert.AreEqual(150.02m, message.Asks[1].Price);
            Assert.AreEqual(250, message.Asks[1].Volume);
        }

        [TestMethod]
        public void QuoteChangeMessage_BestBid_ReturnsHighestBid()
        {
            // Arrange
            var message = new QuoteChangeMessage
            {
                Bids =
                {
                    new QuoteChange { Price = 149.99m, Volume = 200 },
                    new QuoteChange { Price = 150.00m, Volume = 100 },
                    new QuoteChange { Price = 149.98m, Volume = 300 }
                }
            };

            // Act
            var bestBid = message.Bids[0]; // Assuming sorted by price descending

            // Assert
            Assert.AreEqual(150.00m, bestBid.Price);
        }

        [TestMethod]
        public void QuoteChangeMessage_BestAsk_ReturnsLowestAsk()
        {
            // Arrange
            var message = new QuoteChangeMessage
            {
                Asks =
                {
                    new QuoteChange { Price = 150.02m, Volume = 250 },
                    new QuoteChange { Price = 150.01m, Volume = 150 },
                    new QuoteChange { Price = 150.03m, Volume = 350 }
                }
            };

            // Act
            var bestAsk = message.Asks[0]; // Assuming sorted by price ascending

            // Assert
            Assert.AreEqual(150.01m, bestAsk.Price);
        }

        [TestMethod]
        public void QuoteChangeMessage_Spread_CalculatedCorrectly()
        {
            // Arrange
            var message = new QuoteChangeMessage
            {
                Bids = { new QuoteChange { Price = 150.00m, Volume = 100 } },
                Asks = { new QuoteChange { Price = 150.02m, Volume = 150 } }
            };

            // Act
            var spread = message.Asks[0].Price - message.Bids[0].Price;

            // Assert
            Assert.AreEqual(0.02m, spread);
        }

        [TestMethod]
        public void QuoteChangeMessage_MidPrice_CalculatedCorrectly()
        {
            // Arrange
            var message = new QuoteChangeMessage
            {
                Bids = { new QuoteChange { Price = 150.00m, Volume = 100 } },
                Asks = { new QuoteChange { Price = 150.02m, Volume = 150 } }
            };

            // Act
            var midPrice = (message.Bids[0].Price + message.Asks[0].Price) / 2;

            // Assert
            Assert.AreEqual(150.01m, midPrice);
        }
    }
}