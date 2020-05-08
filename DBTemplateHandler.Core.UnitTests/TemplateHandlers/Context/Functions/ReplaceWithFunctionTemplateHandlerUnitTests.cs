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
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ReplaceWithFunctionTemplateHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnArgumentNullExpcetionWhenStringContextIsNull()
        {
            Assert.Throws<Exception>(() => _tested.ProcessContext(null,new ProcessorDatabaseContext() { }));
        }

        [Test]
        public void ShouldReturnEmptyStringWhenInternValueInContextIsStringEmpty()
        {
            var result = _tested.ProcessContext($"{_tested.StartContext}{string.Empty}{_tested.EndContext}",new ProcessorDatabaseContext() { });
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ShouldReturnInternValueWhenInternContextDoesNotContainsWithKeyWord()
        {
            string internContext = "Hello World";
            var result = _tested.ProcessContext($"{_tested.StartContext}{internContext}{_tested.EndContext}",new ProcessorDatabaseContext() { });
            Assert.AreEqual(internContext, result);
        }

        [Test]
        public void ShouldReturnReplacedWhiteSpacesByStringEmptyValue()
        {
            string internContext = "Hello World";
            var processed = $"{_tested.StartContext}{internContext}{_tested.WithSeparator}{" "}{_tested.BySeparator}{String.Empty}{_tested.EndContext}";
            TestContext.Out.WriteLine($"{nameof(processed)}->'{processed}'");
            var result = _tested.ProcessContext(processed,new ProcessorDatabaseContext() { });
            Assert.AreEqual("HelloWorld", result);
        }

    }
}
