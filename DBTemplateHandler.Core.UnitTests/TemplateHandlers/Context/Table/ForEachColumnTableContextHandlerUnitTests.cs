using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class ForEachColumnTableContextHandlerUnitTests
    {

        public ColumnNameColumnContextHandler columnNameContextHandler;
        public ForEachColumnTableContextHandler _tested;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ForEachColumnTableContextHandler(templateHandler);
            columnNameContextHandler = new ColumnNameColumnContextHandler(templateHandler);
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:COLUMN:FOREACH[", _tested.StartContext);
        }

        [Test]
        public void ShouldReturnAccurateEndContextValue()
        {
            Assert.AreEqual("]::}", _tested.EndContext);
        }

        [Test]
        public void isStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldReturnAccurateContextActionDescription()
        {
            Assert.AreEqual("Is replaced by the intern context as many time as there is column in the table", _tested.ContextActionDescription);
        }


        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenTableModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenColumnModelListIsNullInTableModel()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Table = new TableModelForTest() { Columns = null } };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenAtLeastOneColumnModelInColumnsFromTableModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext()
                {
                    Table =
                    new TableModelForTest() { Columns = new List<IColumnModel>() { new ColumnModelForTest() { }, null } }
                };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldReturnEveryColumnNamePresentInTheTable()
        {
            var firstColumnName = "firstColumnName";
            var secondColumnName = "secondColumnName";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                    new TableModelForTest() { Columns = new List<IColumnModel>() 
                    { 
                        new ColumnModelForTest() { Name = firstColumnName }, 
                        new ColumnModelForTest() { Name = secondColumnName },
                    } 
                }
            };
            var templateContext = $"{_tested.StartContext}{columnNameContextHandler.Signature},{_tested.EndContext}";
            var actual = _tested.ProcessContext(templateContext, databaseContext);
            var expected = $"{firstColumnName},{secondColumnName},";
            Assert.AreEqual(expected, actual);
        }
    }
}
