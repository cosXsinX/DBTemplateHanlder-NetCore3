using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Constraints;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class ForEachTablesForeignVisitContextHanlderUnitTests
    {
        TableNameTableContextHandler tableNameTableContextHandler;
        ForEachTablesForeignVisitContextHanlder _tested;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ForEachTablesForeignVisitContextHanlder(templateHandler);
            tableNameTableContextHandler = new TableNameTableContextHandler(templateHandler);
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:CONSTRAINT:CURRENT:VISIT:FOREIGN:TABLE:FOREACH[", _tested.StartContext);
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
            Assert.AreEqual("Is replaced by the intern context as many time as there is foreign table dependance", _tested.ContextActionDescription);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = new TableModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenTableModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenDatabaseModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = new TableModelForTest() { }, Database = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenDatabaseTablesAreNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = new TableModelForTest() { }, Database = new DatabaseModelForTest() { Tables = null }  }));
        }

        private void MakeDependant(ITableModel depending,ITableModel dependence)
        {
            if(depending.ForeignKeyConstraints == null) depending.ForeignKeyConstraints = new List<IForeignKeyConstraintModel>();
            depending.ForeignKeyConstraints.Add(new ForeignKeyConstraintModel()
            {
                Elements = new List<IForeignKeyConstraintElementModel>()
                        {
                            new ForeignKeyConstraintElementModel()
                            {
                                Foreign = new ColumnReferenceModel()
                                {
                                    TableName = dependence.Name
                                }
                            }
                        }
            });
        }

        [Test]
        public void ShouldReturnAccurateTableNameChainWhenThereIsOnlyOneDependence()
        {
            var firstForeignDependencyTable = new TableModelForTest() { Name = "firstForeignDependencyTable", };
            var startPointTable = new TableModelForTest() { Name = "startPointTable" };

            MakeDependant(startPointTable, firstForeignDependencyTable);
            string StringContext = $"{_tested.StartContext}{tableNameTableContextHandler.Signature},{_tested.EndContext}";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table = startPointTable,
                Database = new DatabaseModelForTest()
                {
                    Tables = new List<ITableModel>
                    {
                        firstForeignDependencyTable,
                        startPointTable
                    }
                }
            };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            var expected = $"{firstForeignDependencyTable.Name},";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldReturnAccurateTableNameChainWhenThereIsOnlyTwoDependence()
        {
            var firstForeignDependencyTable = new TableModelForTest() { Name = "firstForeignDependencyTable", };
            var secondForeignDependencyTable = new TableModelForTest() { Name = "secondForeignDependencyTable", };
            var startPointTable = new TableModelForTest() { Name = "startPointTable" };

            MakeDependant(startPointTable, firstForeignDependencyTable);
            MakeDependant(startPointTable, secondForeignDependencyTable);
            string StringContext = $"{_tested.StartContext}{tableNameTableContextHandler.Signature},{_tested.EndContext}";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table = startPointTable,
                Database = new DatabaseModelForTest()
                {
                    Tables = new List<ITableModel>
                    {
                        firstForeignDependencyTable,
                        secondForeignDependencyTable,
                        startPointTable
                    }
                }
            };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            var expected = $"{firstForeignDependencyTable.Name},{secondForeignDependencyTable.Name},";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldReturnAccurateTableNameChainWhenOneIntermediateLevel()
        {
            var rootForeignDependencyTable = new TableModelForTest() { Name = "rootForeignDependencyTable",};
            var firstLevelItermediateForeignDependencyTable = new TableModelForTest(){Name = "firstLevelItermediateForeignDependencyTable",};
            var startPointTable = new TableModelForTest() {Name = "startPointTable"};

            MakeDependant(firstLevelItermediateForeignDependencyTable, rootForeignDependencyTable);
            MakeDependant(startPointTable, firstLevelItermediateForeignDependencyTable);
            string StringContext = $"{_tested.StartContext}{tableNameTableContextHandler.Signature},{_tested.EndContext}";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table = startPointTable,
                Database = new DatabaseModelForTest() 
                { 
                    Tables = new List<ITableModel> 
                    {
                        rootForeignDependencyTable,
                        firstLevelItermediateForeignDependencyTable,
                        startPointTable
                    } 
                }
            };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            var expected = $"{rootForeignDependencyTable.Name},{firstLevelItermediateForeignDependencyTable.Name},";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldReturnAccurateTableNameChainWhenThereIsAFork()
        {
            var firstRootForeignDependencyTable = new TableModelForTest() { Name = "firstRootForeignDependencyTable", };
            var secondRootForeignDependencyTable = new TableModelForTest() { Name = "secondRootForeignDependencyTable", };
            var firstLevelItermediateForeignDependencyTable = new TableModelForTest() { Name = "firstLevelItermediateForeignDependencyTable", };
            var startPointTable = new TableModelForTest() { Name = "startPointTable" };

            MakeDependant(firstLevelItermediateForeignDependencyTable, firstRootForeignDependencyTable);
            MakeDependant(firstLevelItermediateForeignDependencyTable, secondRootForeignDependencyTable);
            MakeDependant(startPointTable, firstLevelItermediateForeignDependencyTable);
            string StringContext = $"{_tested.StartContext}{tableNameTableContextHandler.Signature},{_tested.EndContext}";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table = startPointTable,
                Database = new DatabaseModelForTest()
                {
                    Tables = new List<ITableModel>
                    {
                        firstRootForeignDependencyTable,
                        secondRootForeignDependencyTable,
                        firstLevelItermediateForeignDependencyTable,
                        startPointTable
                    }
                }
            };
            var actual = _tested.ProcessContext(StringContext, databaseContext);
            var expected = $"{firstRootForeignDependencyTable.Name},{secondRootForeignDependencyTable.Name},{firstLevelItermediateForeignDependencyTable.Name},";
            Assert.AreEqual(expected, actual);
        }

    }
}
