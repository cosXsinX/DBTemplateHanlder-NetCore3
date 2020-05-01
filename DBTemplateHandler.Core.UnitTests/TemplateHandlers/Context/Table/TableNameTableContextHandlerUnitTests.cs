using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class TableNameTableContextHandlerUnitTests
    {
        private TableNameTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new TableNameTableContextHandler(templateHandler);
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
                _tested.TableModel = null;
                _tested.processContext("Hello World !");
            }, $"The {nameof(_tested.TableModel)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsNoDatabaseNamePlaceHolder()
        {
            string expectedDatabaseName = "expected table name";
            string StringContext = $"{_tested.StartContext}I Should not be here{_tested.EndContext}";
            Assert.Throws<Exception>(() =>
            {
                _tested.TableModel = new TableModelForTest() { Name = expectedDatabaseName };
                _tested.processContext(StringContext);
            }, $"Exception control no more satified : Expected exception message : There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{_tested.Signature}'" + _tested.Signature + "'");
        }

        [Test]
        public void ShouldReplaceTheTableNameSignatureByTableName()
        {
            string expectedTableName = "ExpectedTableName";
            string StringContext = _tested.Signature;
            string expected = StringContext.Replace(_tested.Signature, expectedTableName);
            _tested.TableModel = new TableModelForTest() { Name = expectedTableName };
            var actual = _tested.processContext(StringContext);
            Assert.AreEqual(expected, actual);
        }


        public class TableModelForTest : ITableModel
        {
            public IList<IColumnModel> Columns { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string Name { get; set; }
            public string Schema { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IDatabaseModel ParentDatabase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }
        }
    }
}
