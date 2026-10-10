using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

namespace StockSharp.Tests.Algo
{
    [TestClass]
    public class StrategyTests
    {
        [TestMethod]
        public void Strategy_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var strategy = new Strategy();

            // Assert
            Assert.IsNotNull(strategy.Id);
            Assert.AreEqual(ProcessStates.Stopped, strategy.ProcessState);
            Assert.IsNotNull(strategy.Parameters);
            Assert.AreEqual(0, strategy.Parameters.Count);
        }

        [TestMethod]
        public void Strategy_Name_CanBeSet()
        {
            // Arrange
            var strategy = new Strategy();
            const string name = "TestStrategy";

            // Act
            strategy.Name = name;

            // Assert
            Assert.AreEqual(name, strategy.Name);
        }

        [TestMethod]
        public void Strategy_Parameter_CanBeAdded()
        {
            // Arrange
            var strategy = new Strategy();

            // Act
            var param = strategy.Param("TestParam", 100);

            // Assert
            Assert.IsNotNull(param);
            Assert.AreEqual("TestParam", param.Name);
            Assert.AreEqual(100, param.Value);
            Assert.AreEqual(1, strategy.Parameters.Count);
        }

        [TestMethod]
        public void Strategy_Parameter_OptimizationRange()
        {
            // Arrange
            var strategy = new Strategy();

            // Act
            var param = strategy.Param("OptimizableParam", 50)
                .SetRange(10, 100, 10);

            // Assert
            Assert.AreEqual(10, param.MinValue);
            Assert.AreEqual(100, param.MaxValue);
            Assert.AreEqual(10, param.Step);
        }

        [TestMethod]
        public void Strategy_ProcessState_Transitions()
        {
            // Arrange
            var strategy = new Strategy();

            // Act & Assert
            Assert.AreEqual(ProcessStates.Stopped, strategy.ProcessState);
            
            // Note: Actual state transitions require connector setup
            // This tests the property setter
            strategy.ProcessState = ProcessStates.Starting;
            Assert.AreEqual(ProcessStates.Starting, strategy.ProcessState);
            
            strategy.ProcessState = ProcessStates.Started;
            Assert.AreEqual(ProcessStates.Started, strategy.ProcessState);
            
            strategy.ProcessState = ProcessStates.Stopping;
            Assert.AreEqual(ProcessStates.Stopping, strategy.ProcessState);
            
            strategy.ProcessState = ProcessStates.Stopped;
            Assert.AreEqual(ProcessStates.Stopped, strategy.ProcessState);
        }

        [TestMethod]
        public void Strategy_ChildStrategies_CanBeAdded()
        {
            // Arrange
            var parent = new Strategy();
            var child = new Strategy { Name = "ChildStrategy" };

            // Act
            parent.ChildStrategies.Add(child);

            // Assert
            Assert.AreEqual(1, parent.ChildStrategies.Count);
            Assert.AreSame(child, parent.ChildStrategies[0]);
        }

        [TestMethod]
        public void Strategy_RiskRules_CanBeAdded()
        {
            // Arrange
            var strategy = new Strategy();

            // Act
            var riskRule = new RiskRule
            {
                Action = RiskActions.StopStrategy,
                Condition = () => strategy.PnL < -1000
            };
            strategy.RiskRules.Add(riskRule);

            // Assert
            Assert.AreEqual(1, strategy.RiskRules.Count);
        }

        [TestMethod]
        public void Strategy_PnL_TracksProfitAndLoss()
        {
            // Arrange
            var strategy = new Strategy();

            // Act
            strategy.PnL = 1500m;

            // Assert
            Assert.AreEqual(1500m, strategy.PnL);
        }

        [TestMethod]
        public void Strategy_CommissionManager_CanBeSet()
        {
            // Arrange
            var strategy = new Strategy();
            var commissionManager = new CommissionManager();

            // Act
            strategy.CommissionManager = commissionManager;

            // Assert
            Assert.AreSame(commissionManager, strategy.CommissionManager);
        }
    }
}