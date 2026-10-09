using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Commissions;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class CommissionManagerTests
    {
        [TestMethod]
        public void CommissionManager_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var commissionManager = new CommissionManager();

            // Assert
            Assert.IsNotNull(commissionManager.Rules);
            Assert.AreEqual(0, commissionManager.Rules.Count);
        }

        [TestMethod]
        public void CommissionRule_PerTrade_CalculatesCorrectly()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 1.0m, // $1 per trade
                RuleType = CommissionRuleTypes.PerTrade
            };
            var order = new Order { Volume = 100, Price = 150m };
            var trade = new Trade { Volume = 100, Price = 150m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            Assert.AreEqual(1.0m, commission);
        }

        [TestMethod]
        public void CommissionRule_PerVolume_CalculatesCorrectly()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 0.01m, // 1 cent per share
                RuleType = CommissionRuleTypes.PerVolume
            };
            var order = new Order { Volume = 1000, Price = 150m };
            var trade = new Trade { Volume = 1000, Price = 150m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            Assert.AreEqual(10.0m, commission); // 1000 * 0.01
        }

        [TestMethod]
        public void CommissionRule_Percent_CalculatesCorrectly()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 0.001m, // 0.1% of trade value
                RuleType = CommissionRuleTypes.Percent
            };
            var order = new Order { Volume = 100, Price = 150m };
            var trade = new Trade { Volume = 100, Price = 150m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            var tradeValue = 100 * 150m; // 15000
            var expectedCommission = tradeValue * 0.001m; // 15
            Assert.AreEqual(expectedCommission, commission);
        }

        [TestMethod]
        public void CommissionRule_Tiered_CalculatesCorrectly()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 0.005m, // 0.5% base
                RuleType = CommissionRuleTypes.Tiered,
                MinCommission = 1.0m,
                MaxCommission = 100.0m
            };
            var order = new Order { Volume = 100, Price = 150m };
            var trade = new Trade { Volume = 100, Price = 150m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            var tradeValue = 100 * 150m; // 15000
            var baseCommission = tradeValue * 0.005m; // 75
            Assert.AreEqual(Math.Max(1.0m, Math.Min(100.0m, baseCommission)), commission);
        }

        [TestMethod]
        public void CommissionManager_MultipleRules_SumsCommissions()
        {
            // Arrange
            var manager = new CommissionManager();
            manager.Rules.Add(new CommissionRule { Commission = 1.0m, RuleType = CommissionRuleTypes.PerTrade });
            manager.Rules.Add(new CommissionRule { Commission = 0.01m, RuleType = CommissionRuleTypes.PerVolume });
            
            var order = new Order { Volume = 100, Price = 150m };
            var trade = new Trade { Volume = 100, Price = 150m };

            // Act
            var totalCommission = manager.Calculate(order, trade);

            // Assert
            Assert.AreEqual(1.0m + 1.0m, totalCommission); // 1 (per trade) + 1 (per volume: 100*0.01)
        }

        [TestMethod]
        public void CommissionRule_MinCommission_Enforced()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 0.0001m, // Very small percentage
                RuleType = CommissionRuleTypes.Percent,
                MinCommission = 5.0m
            };
            var order = new Order { Volume = 1, Price = 100m };
            var trade = new Trade { Volume = 1, Price = 100m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            Assert.AreEqual(5.0m, commission); // Min commission enforced
        }

        [TestMethod]
        public void CommissionRule_MaxCommission_Enforced()
        {
            // Arrange
            var rule = new CommissionRule
            {
                Commission = 0.1m, // 10% - very high
                RuleType = CommissionRuleTypes.Percent,
                MaxCommission = 50.0m
            };
            var order = new Order { Volume = 10000, Price = 100m };
            var trade = new Trade { Volume = 10000, Price = 100m };

            // Act
            var commission = rule.Calculate(order, trade);

            // Assert
            Assert.AreEqual(50.0m, commission); // Max commission enforced
        }
    }
}