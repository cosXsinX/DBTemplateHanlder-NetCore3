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
    public class WhenHasIndexColumnTableContextHandlerUnitTests
    {
        private WhenHasIndexColumnTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new WhenHasIndexColumnTableContextHandler(templateHandler);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }


        [Test]
        public void ShouldThrowANExceptionWhenColumnModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                _tested.ProcessContext("Hello World !", new ProcessorDatabaseContext() { Table = null});
            });
        }

        [Test]
        public void ShouldNotReturnContextContentWhenTheTableHasNotIndexedColumn()
        {
            var databaseContext = new ProcessorDatabaseContext() { Table =
                new TableModelForTest() { 
                    Columns = new List<IColumnModel>() 
                    { 
                        new ColumnModelForTest() { IsIndexed = false, }
                    } 
                }
            };
            var contextContent = "Hello World !";
            var result = _tested.ProcessContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(String.Empty, result);
        }

        [Test]
        public void ShouldReturnContextContentWhenTheTableHasIndexedColumn()
        {
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                new TableModelForTest()
                {
                    Columns = new List<IColumnModel>()
                    {
                        new ColumnModelForTest() { IsIndexed = true, }
                    }
                }
            };
            var contextContent = "Hello World !";
            var result = _tested.ProcessContext($"{_tested.StartContext}{contextContent}{_tested.EndContext}", databaseContext);
            Assert.AreEqual(contextContent, result);
        }
    }
}
