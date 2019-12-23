using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Functions
{
    [TestFixture]
    public class ReplaceWithFunctionTemplateHandlerUnitTests
    {
        private ReplaceWithFunctionTemplateHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            _tested = new ReplaceWithFunctionTemplateHandler(templateHandlerNew);
        }

        [Test]
        public void ShouldThrowAnArgumentNullExpcetionWhenStringContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.processContext(null));
        }

        //TODO implement missing Unit tests
    }
}
