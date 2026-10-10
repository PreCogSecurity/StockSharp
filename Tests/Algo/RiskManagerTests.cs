using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Risk;
using StockSharp.BusinessEntities;
using StockSharp.Algo.Strategies;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class RiskManagerTests
    {
        [TestMethod]
        public void RiskManager_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var riskManager = new RiskManager();

            // Assert
            Assert.IsNotNull(riskManager.Rules);
            Assert.AreEqual(0, riskManager.Rules.Count);
        }

        [TestMethod]
        public void RiskManager_AddRule_CanAddRules()
        {
            // Arrange
            var riskManager = new RiskManager();
            var rule = new RiskRule
            {
                Action = RiskActions.StopStrategy,
                Condition = () => true
            };

            // Act
            riskManager.Rules.Add(rule);

            // Assert
            Assert.AreEqual(1, riskManager.Rules.Count);
        }

        [TestMethod]
        public void RiskRule_Condition_EvaluatesCorrectly()
        {
            // Arrange
            var strategy = new Strategy();
            var rule = new RiskRule
            {
                Action = RiskActions.StopStrategy,
                Condition = () => strategy.PnL < -1000
            };

            // Act & Assert
            strategy.PnL = 500;
            Assert.IsFalse(rule.Condition());

            strategy.PnL = -1500;
            Assert.IsTrue(rule.Condition());
        }

        [TestMethod]
        public void RiskRule_Actions_AreDefined()
        {
            // Arrange & Act & Assert
            Assert.AreEqual(RiskActions.StopStrategy, RiskActions.StopStrategy);
            Assert.AreEqual(RiskActions.ClosePositions, RiskActions.ClosePositions);
            Assert.AreEqual(RiskActions.CancelOrders, RiskActions.CancelOrders);
            Assert.AreEqual(RiskActions.Warning, RiskActions.Warning);
        }

        [TestMethod]
        public void PositionLimitRule_EnforcesMaxPosition()
        {
            // Arrange
            var strategy = new Strategy();
            var position = new Position { CurrentValue = 100 };
            var rule = new RiskRule
            {
                Action = RiskActions.ClosePositions,
                Condition = () => Math.Abs(position.CurrentValue) > 500
            };

            // Act & Assert
            position.CurrentValue = 300;
            Assert.IsFalse(rule.Condition());

            position.CurrentValue = 600;
            Assert.IsTrue(rule.Condition());

            position.CurrentValue = -600;
            Assert.IsTrue(rule.Condition());
        }

        [TestMethod]
        public void DrawdownRule_EnforcesMaxDrawdown()
        {
            // Arrange
            var strategy = new Strategy();
            var peakPnL = 10000m;
            var rule = new RiskRule
            {
                Action = RiskActions.StopStrategy,
                Condition = () => (peakPnL - strategy.PnL) / peakPnL > 0.1m // 10% drawdown
            };

            // Act & Assert
            strategy.PnL = 9500; // 5% drawdown
            Assert.IsFalse(rule.Condition());

            strategy.PnL = 8500; // 15% drawdown
            Assert.IsTrue(rule.Condition());
        }

        [TestMethod]
        public void DailyLossLimitRule_EnforcesDailyLimit()
        {
            // Arrange
            var strategy = new Strategy();
            var dailyStartPnL = 0m;
            var rule = new RiskRule
            {
                Action = RiskActions.StopStrategy,
                Condition = () => (dailyStartPnL - strategy.PnL) > 2000 // $2000 daily loss limit
            };

            // Act & Assert
            strategy.PnL = -1000;
            Assert.IsFalse(rule.Condition());

            strategy.PnL = -2500;
            Assert.IsTrue(rule.Condition());
        }

        [TestMethod]
        public void MarginRule_EnforcesMarginRequirements()
        {
            // Arrange
            var portfolio = new Portfolio { CurrentValue = 100000m, BlockedValue = 50000m };
            var rule = new RiskRule
            {
                Action = RiskActions.Warning,
                Condition = () => portfolio.BlockedValue / portfolio.CurrentValue > 0.8m // 80% margin usage
            };

            // Act & Assert
            portfolio.BlockedValue = 50000; // 50% usage
            Assert.IsFalse(rule.Condition());

            portfolio.BlockedValue = 85000; // 85% usage
            Assert.IsTrue(rule.Condition());
        }
    }
}