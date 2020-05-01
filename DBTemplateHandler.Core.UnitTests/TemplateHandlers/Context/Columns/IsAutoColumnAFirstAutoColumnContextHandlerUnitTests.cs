using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class IsAutoColumnAFirstAutoColumnContextHandlerUnitTests
    {
        private IsAutoColumnAFirstAutoColumnContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new IsAutoColumnAFirstAutoColumnContextHandler(TemplateHandlerBuilder.Build(null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ContextActionDescriptionShouldReturnTheAccurateDescription()
        {
            Assert.AreEqual("Is replaced by the inner context when the current column is the first column from the iterated auto generated value column collection", _tested.ContextActionDescription);
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.processContext(null));
        }

        [Test]
        public void ProcessContextShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            _tested.ColumnModel = null;
            Assert.Throws<Exception>(() => _tested.processContext("Hello"));
        }
    }
}
