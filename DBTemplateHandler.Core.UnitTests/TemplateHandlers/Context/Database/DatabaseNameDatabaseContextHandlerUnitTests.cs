using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Database
{
    [TestFixture]
    public class DatabaseNameDatabaseContextHandlerUnitTests
    {
        private DatabaseNameDatabaseContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new DatabaseNameDatabaseContextHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Database = new DatabaseModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenDatabaseModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Database = null };
                _tested.ProcessContext("Hello World !",databaseContext);
            }, $"The {nameof(IDatabaseContext.Database)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsNoDatabaseNamePlaceHolder()
        {
            string expectedDatabaseName = "expected database name";
            string StringContext = $"{_tested.StartContext}I Should not be here{_tested.EndContext}";
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Database = new DatabaseModelForTest() { Name = expectedDatabaseName } };
                _tested.ProcessContext(StringContext, databaseContext);
            }, "Exception control no more satified : Expected exception message There is a problem with the provided StringContext :'" + StringContext + "' to the suited word '" + _tested.Signature + "'");
        }

        [Test]
        public void ShouldReplaceTheDatabaseNameSignatureByDatabaseName()
        {
            string expectedDatabaseName = "ExpectedDatabaseName";
            string StringContext = _tested.Signature;
            string expected = StringContext.Replace(_tested.Signature, expectedDatabaseName);
            var databaseContext = new ProcessorDatabaseContext() { Database = new DatabaseModelForTest() { Name = expectedDatabaseName } };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
