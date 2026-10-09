using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Candles;
using StockSharp.Messages;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class CandleTests
    {
        [TestMethod]
        public void Candle_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var candle = new CandleMessage();

            // Assert
            Assert.AreEqual(MessageTypes.Candle, candle.Type);
            Assert.AreEqual(TimeSpan.Zero, candle.Arg);
            Assert.AreEqual(DateTimeOffset.MinValue, candle.OpenTime);
            Assert.AreEqual(DateTimeOffset.MinValue, candle.CloseTime);
            Assert.AreEqual(0, candle.OpenPrice);
            Assert.AreEqual(0, candle.HighPrice);
            Assert.AreEqual(0, candle.LowPrice);
            Assert.AreEqual(0, candle.ClosePrice);
            Assert.AreEqual(0, candle.TotalVolume);
        }

        [TestMethod]
        public void Candle_OHLC_CanBeSet()
        {
            // Arrange
            var candle = new CandleMessage
            {
                OpenPrice = 100m,
                HighPrice = 105m,
                LowPrice = 98m,
                ClosePrice = 103m
            };

            // Assert
            Assert.AreEqual(100m, candle.OpenPrice);
            Assert.AreEqual(105m, candle.HighPrice);
            Assert.AreEqual(98m, candle.LowPrice);
            Assert.AreEqual(103m, candle.ClosePrice);
        }

        [TestMethod]
        public void Candle_Volume_CanBeSet()
        {
            // Arrange
            var candle = new CandleMessage
            {
                TotalVolume = 10000
            };

            // Assert
            Assert.AreEqual(10000, candle.TotalVolume);
        }

        [TestMethod]
        public void Candle_TimeFrame_CanBeSet()
        {
            // Arrange
            var candle = new CandleMessage
            {
                Arg = TimeSpan.FromMinutes(5)
            };

            // Assert
            Assert.AreEqual(TimeSpan.FromMinutes(5), candle.Arg);
        }

        [TestMethod]
        public void Candle_State_Transitions()
        {
            // Arrange
            var candle = new CandleMessage();

            // Act & Assert
            Assert.AreEqual(CandleStates.None, candle.State);
            
            candle.State = CandleStates.Active;
            Assert.AreEqual(CandleStates.Active, candle.State);
            
            candle.State = CandleStates.Finished;
            Assert.AreEqual(CandleStates.Finished, candle.State);
        }

        [TestMethod]
        public void Candle_SecurityId_CanBeSet()
        {
            // Arrange
            var candle = new CandleMessage
            {
                SecurityId = new SecurityId { SecurityCode = "AAPL", BoardCode = "NASDAQ" }
            };

            // Assert
            Assert.AreEqual("AAPL", candle.SecurityId.SecurityCode);
            Assert.AreEqual("NASDAQ", candle.SecurityId.BoardCode);
        }

        [TestMethod]
        public void Candle_PriceRange_CalculatedCorrectly()
        {
            // Arrange
            var candle = new CandleMessage
            {
                HighPrice = 105m,
                LowPrice = 98m
            };

            // Act
            var range = candle.HighPrice - candle.LowPrice;

            // Assert
            Assert.AreEqual(7m, range);
        }

        [TestMethod]
        public void Candle_BodySize_CalculatedCorrectly()
        {
            // Arrange
            var candle = new CandleMessage
            {
                OpenPrice = 100m,
                ClosePrice = 103m
            };

            // Act
            var bodySize = Math.Abs(candle.ClosePrice - candle.OpenPrice);

            // Assert
            Assert.AreEqual(3m, bodySize);
        }

        [TestMethod]
        public void Candle_IsBullish_WhenCloseGreaterThanOpen()
        {
            // Arrange
            var candle = new CandleMessage
            {
                OpenPrice = 100m,
                ClosePrice = 103m
            };

            // Act
            var isBullish = candle.ClosePrice > candle.OpenPrice;

            // Assert
            Assert.IsTrue(isBullish);
        }

        [TestMethod]
        public void Candle_IsBearish_WhenCloseLessThanOpen()
        {
            // Arrange
            var candle = new CandleMessage
            {
                OpenPrice = 103m,
                ClosePrice = 100m
            };

            // Act
            var isBearish = candle.ClosePrice < candle.OpenPrice;

            // Assert
            Assert.IsTrue(isBearish);
        }

        [TestMethod]
        public void Candle_IsDoji_WhenOpenEqualsClose()
        {
            // Arrange
            var candle = new CandleMessage
            {
                OpenPrice = 100m,
                ClosePrice = 100m
            };

            // Act
            var isDoji = candle.OpenPrice == candle.ClosePrice;

            // Assert
            Assert.IsTrue(isDoji);
        }
    }
}