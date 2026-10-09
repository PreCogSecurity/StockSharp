using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockSharp.Messages;

namespace StockSharp.Tests.Messages
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void Message_DefaultConstructor_SetsProperties()
        {
            // Arrange & Act
            var message = new Message();

            // Assert
            Assert.IsNotNull(message.Type);
            Assert.IsNotNull(message.LocalTime);
            Assert.AreEqual(0, message.ExtensionInfo?.Count ?? 0);
        }

        [TestMethod]
        public void Message_Type_CanBeSet()
        {
            // Arrange
            var message = new Message();

            // Act
            message.Type = MessageTypes.Connect;

            // Assert
            Assert.AreEqual(MessageTypes.Connect, message.Type);
        }

        [TestMethod]
        public void Message_LocalTime_DefaultsToNow()
        {
            // Arrange
            var before = DateTimeOffset.Now;

            // Act
            var message = new Message();

            // Assert
            var after = DateTimeOffset.Now;
            Assert.IsTrue(message.LocalTime >= before && message.LocalTime <= after);
        }

        [TestMethod]
        public void Message_ExtensionInfo_CanStoreCustomData()
        {
            // Arrange
            var message = new Message();
            const string key = "CustomKey";
            const string value = "CustomValue";

            // Act
            message.ExtensionInfo[key] = value;

            // Assert
            Assert.IsTrue(message.ExtensionInfo.ContainsKey(key));
            Assert.AreEqual(value, message.ExtensionInfo[key]);
        }

        [TestMethod]
        public void Message_Clone_CreatesIndependentCopy()
        {
            // Arrange
            var original = new Message
            {
                Type = MessageTypes.Security,
                LocalTime = DateTimeOffset.Now,
                ExtensionInfo = { { "Test", "Value" } }
            };

            // Act
            var clone = original.Clone();

            // Assert
            Assert.AreEqual(original.Type, clone.Type);
            Assert.AreEqual(original.LocalTime, clone.LocalTime);
            Assert.AreEqual(original.ExtensionInfo.Count, clone.ExtensionInfo.Count);
            Assert.AreEqual(original.ExtensionInfo["Test"], clone.ExtensionInfo["Test"]);
            
            // Verify independence
            clone.ExtensionInfo["Test"] = "Modified";
            Assert.AreNotEqual(original.ExtensionInfo["Test"], clone.ExtensionInfo["Test"]);
        }
    }
}