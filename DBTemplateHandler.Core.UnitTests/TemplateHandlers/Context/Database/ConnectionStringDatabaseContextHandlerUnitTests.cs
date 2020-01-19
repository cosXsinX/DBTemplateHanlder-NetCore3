using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Database
{
    [TestFixture]
    public class ConnectionStringDatabaseContextHandlerUnitTests
    {
        private ConnectionStringDatabaseContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            _tested = new ConnectionStringDatabaseContextHandler(templateHandlerNew);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenDatabaseModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.DatabaseModel = null;
                _tested.processContext("Hello World !");
            }, $"The {nameof(_tested.DatabaseModel)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsNoDatabaseNamePlaceHolder()
        {
            string expectedDatabaseName = "expected database name";
            string StringContext = $"{_tested.StartContext}I Should not be here{_tested.EndContext}";
            Assert.Throws<Exception>(() =>
            {
                _tested.DatabaseModel = new DatabaseModelForTest() { Name = expectedDatabaseName };
                _tested.processContext(StringContext);
            }, "Exception control no more satified : Expected exception message There is a problem with the provided StringContext :'" + StringContext + "' to the suited word '" + _tested.Signature + "'");
        }

        [Test]
        public void ShouldReplaceTheDatabaseNameSignatureByDatabaseName()
        {
            string expectedDatabaseName = "ExpectedDatabaseName";
            string StringContext = _tested.Signature;
            string expected = StringContext.Replace(_tested.Signature, expectedDatabaseName);
            _tested.DatabaseModel = new DatabaseModelForTest() { ConnectionString = expectedDatabaseName };
            var actual = _tested.processContext(StringContext);
            Assert.AreEqual(expected, actual);
        }


        public class DatabaseModelForTest : IDatabaseModel
        {
            public string TypeSetName { get; set; }
            public string Name { get; set; }
            public IList<ITableModel> Tables { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string ConnectionString { get; set; }
        }
    }
}
