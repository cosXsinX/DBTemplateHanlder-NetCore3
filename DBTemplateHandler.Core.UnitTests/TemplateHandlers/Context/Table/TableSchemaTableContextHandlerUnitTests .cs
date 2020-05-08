using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class TableSchemaTableContextHandlerUnitTests
    {
        private TableSchemaTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new TableSchemaTableContextHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenDatabaseModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsNoDatabaseNamePlaceHolder()
        {
            string expectedSchemaName = "expected schema name";
            string StringContext = $"{_tested.StartContext}I Should not be here{_tested.EndContext}";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = new TableModelForTest() { Schema = expectedSchemaName } })
            , $"Exception control no more satified : Expected exception message : There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{_tested.Signature}'" + _tested.Signature + "'");
        }

        [Test]
        public void ShouldReplaceTheTableNameSignatureByTableName()
        {
            string expectedTableSchema = "ExpectedTableName";
            string StringContext = _tested.Signature;
            string expected = StringContext.Replace(_tested.Signature, expectedTableSchema);
            var databaseContext = new ProcessorDatabaseContext() { Table = new TableModelForTest() { Schema = expectedTableSchema } };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
