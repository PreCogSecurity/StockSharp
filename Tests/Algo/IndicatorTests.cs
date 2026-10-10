using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Indicators;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class IndicatorTests
    {
        [TestMethod]
        public void SimpleMovingAverage_CalculatesCorrectly()
        {
            // Arrange
            var sma = new SimpleMovingAverage { Length = 3 };
            var values = new decimal[] { 10, 20, 30, 40, 50 };

            // Act
            foreach (var value in values)
            {
                sma.Process(new DecimalIndicatorValue(value));
            }

            // Assert
            // SMA(3) of [10,20,30,40,50] = (30+40+50)/3 = 40
            Assert.AreEqual(40m, sma.GetCurrentValue());
        }

        [TestMethod]
        public void ExponentialMovingAverage_CalculatesCorrectly()
        {
            // Arrange
            var ema = new ExponentialMovingAverage { Length = 3 };
            var values = new decimal[] { 10, 20, 30, 40, 50 };

            // Act
            foreach (var value in values)
            {
                ema.Process(new DecimalIndicatorValue(value));
            }

            // Assert
            // EMA should be weighted towards recent values
            var currentValue = ema.GetCurrentValue();
            Assert.IsTrue(currentValue > 30m && currentValue < 50m);
        }

        [TestMethod]
        public void RelativeStrengthIndex_CalculatesCorrectly()
        {
            // Arrange
            var rsi = new RelativeStrengthIndex { Length = 14 };
            // Simulate uptrend
            var values = new decimal[] 
            { 
                44, 44.34m, 44.09m, 43.61m, 44.33m, 44.83m, 45.10m, 45.42m, 
                45.84m, 46.08m, 45.89m, 46.03m, 45.61m, 46.28m, 46.28m, 
                46.00m, 46.03m, 46.41m, 46.22m, 45.64m
            };

            // Act
            foreach (var value in values)
            {
                rsi.Process(new DecimalIndicatorValue(value));
            }

            // Assert
            var currentValue = rsi.GetCurrentValue();
            // RSI should be between 0 and 100
            Assert.IsTrue(currentValue >= 0 && currentValue <= 100);
            // With uptrend, RSI should be > 50
            Assert.IsTrue(currentValue > 50);
        }

        [TestMethod]
        public void BollingerBands_CalculatesCorrectly()
        {
            // Arrange
            var bb = new BollingerBands { Length = 20, Width = 2 };
            var random = new Random(42);
            var values = new decimal[25];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 100 + (decimal)(random.NextDouble() * 10 - 5); // Random around 100
            }

            // Act
            foreach (var value in values)
            {
                bb.Process(new DecimalIndicatorValue(value));
            }

            // Assert
            var currentValue = bb.GetCurrentValue();
            Assert.IsNotNull(currentValue);
            
            // BollingerBands returns a tuple (Middle, Upper, Lower)
            // We just verify it produces a value
            Assert.IsInstanceOfType(currentValue, typeof(ValueTuple<decimal, decimal, decimal>));
        }

        [TestMethod]
        public void AverageTrueRange_CalculatesCorrectly()
        {
            // Arrange
            var atr = new AverageTrueRange { Length = 14 };
            // Simulate OHLC data
            var highs = new decimal[] { 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119 };
            var lows = new decimal[] { 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
            var closes = new decimal[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114 };

            // Act
            for (int i = 0; i < highs.Length; i++)
            {
                var candle = new CandleMessage
                {
                    HighPrice = highs[i],
                    LowPrice = lows[i],
                    ClosePrice = closes[i],
                    OpenPrice = closes[i] - 1
                };
                atr.Process(candle);
            }

            // Assert
            var currentValue = atr.GetCurrentValue();
            Assert.IsTrue(currentValue > 0);
        }

        [TestMethod]
        public void Indicator_Reset_ClearsState()
        {
            // Arrange
            var sma = new SimpleMovingAverage { Length = 3 };
            sma.Process(new DecimalIndicatorValue(10));
            sma.Process(new DecimalIndicatorValue(20));
            sma.Process(new DecimalIndicatorValue(30));

            // Act
            sma.Reset();

            // Assert
            Assert.IsFalse(sma.IsFormed);
        }

        [TestMethod]
        public void Indicator_IsFormed_AfterEnoughData()
        {
            // Arrange
            var sma = new SimpleMovingAverage { Length = 3 };

            // Act & Assert
            Assert.IsFalse(sma.IsFormed);
            
            sma.Process(new DecimalIndicatorValue(10));
            Assert.IsFalse(sma.IsFormed);
            
            sma.Process(new DecimalIndicatorValue(20));
            Assert.IsFalse(sma.IsFormed);
            
            sma.Process(new DecimalIndicatorValue(30));
            Assert.IsTrue(sma.IsFormed);
        }
    }
}