using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Derivatives;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class BlackScholesTests
    {
        private const double Tolerance = 0.01; // 1 cent tolerance

        [TestMethod]
        public void BlackScholes_CallOption_CalculatesCorrectly()
        {
            // Arrange - Standard test case
            // S=100, K=100, T=1, r=0.05, σ=0.2
            // Expected call price ≈ 10.45
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callPrice = bs.CallPrice;

            // Assert
            Assert.AreEqual(10.45, callPrice, Tolerance);
        }

        [TestMethod]
        public void BlackScholes_PutOption_CalculatesCorrectly()
        {
            // Arrange - Same parameters, put price via put-call parity
            // Put = Call - S + K*e^(-rT) = 10.45 - 100 + 100*e^(-0.05) ≈ 5.57
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var putPrice = bs.PutPrice;

            // Assert
            Assert.AreEqual(5.57, putPrice, Tolerance);
        }

        [TestMethod]
        public void BlackScholes_DeepInTheMoney_Call()
        {
            // Arrange - Deep ITM call should be near intrinsic value
            // S=150, K=100, intrinsic = 50
            var bs = new BlackScholes
            {
                UnderlyingPrice = 150,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callPrice = bs.CallPrice;

            // Assert - Should be close to intrinsic value (50) plus time value
            Assert.IsTrue(callPrice > 50 && callPrice < 55);
        }

        [TestMethod]
        public void BlackScholes_DeepOutOfTheMoney_Call()
        {
            // Arrange - Deep OTM call should be near zero
            // S=50, K=100
            var bs = new BlackScholes
            {
                UnderlyingPrice = 50,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callPrice = bs.CallPrice;

            // Assert - Should be very small
            Assert.IsTrue(callPrice >= 0 && callPrice < 1);
        }

        [TestMethod]
        public void BlackScholes_ZeroVolatility_ReturnsIntrinsicValue()
        {
            // Arrange - Zero volatility means no time value
            var bs = new BlackScholes
            {
                UnderlyingPrice = 110,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.0
            };

            // Act
            var callPrice = bs.CallPrice;
            var putPrice = bs.PutPrice;

            // Assert - Call = max(S-K, 0) = 10, Put = max(K-S, 0) = 0
            Assert.AreEqual(10.0, callPrice, Tolerance);
            Assert.AreEqual(0.0, putPrice, Tolerance);
        }

        [TestMethod]
        public void BlackScholes_Delta_Call_Ranges0To1()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var delta = bs.CallDelta;

            // Assert - Call delta between 0 and 1
            Assert.IsTrue(delta >= 0 && delta <= 1);
            // ATM call delta ≈ 0.5
            Assert.AreEqual(0.5, delta, 0.1);
        }

        [TestMethod]
        public void BlackScholes_Delta_Put_RangesMinus1To0()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var delta = bs.PutDelta;

            // Assert - Put delta between -1 and 0
            Assert.IsTrue(delta >= -1 && delta <= 0);
            // ATM put delta ≈ -0.5
            Assert.AreEqual(-0.5, delta, 0.1);
        }

        [TestMethod]
        public void BlackScholes_Gamma_Positive()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var gamma = bs.Gamma;

            // Assert - Gamma always positive
            Assert.IsTrue(gamma > 0);
        }

        [TestMethod]
        public void BlackScholes_Vega_Positive()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var vega = bs.Vega;

            // Assert - Vega always positive
            Assert.IsTrue(vega > 0);
        }

        [TestMethod]
        public void BlackScholes_Theta_NegativeForLongOptions()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callTheta = bs.CallTheta;
            var putTheta = bs.PutTheta;

            // Assert - Theta negative for long options (time decay)
            Assert.IsTrue(callTheta < 0);
            Assert.IsTrue(putTheta < 0);
        }

        [TestMethod]
        public void BlackScholes_Rho_CallPositive_PutNegative()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callRho = bs.CallRho;
            var putRho = bs.PutRho;

            // Assert
            Assert.IsTrue(callRho > 0); // Call rho positive
            Assert.IsTrue(putRho < 0);  // Put rho negative
        }

        [TestMethod]
        public void BlackScholes_PutCallParity_Holds()
        {
            // Arrange
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05,
                Volatility = 0.2
            };

            // Act
            var callPrice = bs.CallPrice;
            var putPrice = bs.PutPrice;
            var intrinsicDiff = bs.UnderlyingPrice - bs.Strike * Math.Exp(-bs.RiskFreeRate * bs.TimeToExpiry);

            // Assert - Put-Call Parity: Call - Put = S - K*e^(-rT)
            Assert.AreEqual(intrinsicDiff, callPrice - putPrice, Tolerance);
        }

        [TestMethod]
        public void BlackScholes_ImpliedVolatility_CanBeCalculated()
        {
            // Arrange - Use known price to find implied vol
            var bs = new BlackScholes
            {
                UnderlyingPrice = 100,
                Strike = 100,
                TimeToExpiry = 1.0,
                RiskFreeRate = 0.05
            };

            // Set a known call price (from 20% vol)
            bs.CallPrice = 10.45;

            // Act
            var impliedVol = bs.ImpliedVolatility;

            // Assert - Should recover ~20% volatility
            Assert.AreEqual(0.2, impliedVol, 0.02);
        }
    }
}