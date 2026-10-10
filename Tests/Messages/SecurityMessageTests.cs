using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Messages;
using StockSharp.BusinessEntities;

namespace StockSharp.Tests.Messages
{
    [TestClass]
    public class SecurityMessageTests
    {
        [TestMethod]
        public void SecurityMessage_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var message = new SecurityMessage();

            // Assert
            Assert.AreEqual(MessageTypes.Security, message.Type);
            Assert.IsNotNull(message.SecurityId);
            Assert.AreEqual(string.Empty, message.SecurityId.SecurityCode);
            Assert.AreEqual(string.Empty, message.SecurityId.BoardCode);
        }

        [TestMethod]
        public void SecurityMessage_SecurityId_CanBeSet()
        {
            // Arrange
            var message = new SecurityMessage();
            var securityId = new SecurityId { SecurityCode = "AAPL", BoardCode = "NASDAQ" };

            // Act
            message.SecurityId = securityId;

            // Assert
            Assert.AreEqual("AAPL", message.SecurityId.SecurityCode);
            Assert.AreEqual("NASDAQ", message.SecurityId.BoardCode);
        }

        [TestMethod]
        public void SecurityMessage_Name_CanBeSet()
        {
            // Arrange
            var message = new SecurityMessage();
            const string name = "Apple Inc.";

            // Act
            message.Name = name;

            // Assert
            Assert.AreEqual(name, message.Name);
        }

        [TestMethod]
        public void SecurityMessage_PriceStep_CanBeSet()
        {
            // Arrange
            var message = new SecurityMessage();

            // Act
            message.PriceStep = 0.01m;

            // Assert
            Assert.AreEqual(0.01m, message.PriceStep);
        }

        [TestMethod]
        public void SecurityMessage_Type_CanBeSet()
        {
            // Arrange
            var message = new SecurityMessage();

            // Act
            message.SecurityType = SecurityTypes.Stock;

            // Assert
            Assert.AreEqual(SecurityTypes.Stock, message.SecurityType);
        }

        [TestMethod]
        public void SecurityMessage_ToSecurity_CreatesBusinessEntity()
        {
            // Arrange
            var message = new SecurityMessage
            {
                SecurityId = new SecurityId { SecurityCode = "AAPL", BoardCode = "NASDAQ" },
                Name = "Apple Inc.",
                PriceStep = 0.01m,
                SecurityType = SecurityTypes.Stock,
                Decimals = 2,
                VolumeStep = 1,
                MinPrice = 0,
                MaxPrice = 10000,
                Multiplier = 1
            };

            // Act
            var security = message.ToSecurity();

            // Assert
            Assert.IsNotNull(security);
            Assert.AreEqual("AAPL", security.Code);
            Assert.AreEqual("Apple Inc.", security.Name);
            Assert.AreEqual(0.01m, security.PriceStep);
            Assert.AreEqual(SecurityTypes.Stock, security.Type);
            Assert.AreEqual(2, security.Decimals);
            Assert.AreEqual(1, security.VolumeStep);
        }
    }
}