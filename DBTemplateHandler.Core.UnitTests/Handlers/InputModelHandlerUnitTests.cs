using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.Handlers
{
    [TestFixture]
    public class InputModelHandlerUnitTests
    {
        public InputModelHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new InputModelHandler();
        }

        [Test]
        public void ShouldReturnDatabaseNameAndTableName()
        {
            var templateHandlerNew = new TemplateHandlerNew(null);
            DatabaseNameDatabaseContextHandler databaseNameDatabaseContextHandler = new DatabaseNameDatabaseContextHandler(templateHandlerNew);
            TableNameTableContextHandler tableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            string databaseName = "DatabaseName";
            string tableName = "TableName";
            DatabaseTemplateHandlerInputModel input = new DatabaseTemplateHandlerInputModel()
            {
                TemplateModels = new List<ITemplateModel>
                {
                    new TemplateModel()
                    {
                        TemplatedFilePath = $"{_tested.DatabaseFilePathTemplateWord}-{_tested.TableFilePathTemplateWord}",
                        TemplatedFileContent = $"{databaseNameDatabaseContextHandler.Signature}-{tableNameTableContextHandler.Signature}",
                    }
                },
                DatabaseModel = new DatabaseModel()
                {
                    Name = databaseName,
                    Tables = new List<ITableModel>(){ new TableModel()
                    {
                        Name = tableName,
                        Columns = new List<IColumnModel>(),
                    } },
                }
            };

            var result = _tested.Process(input);
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            var resultItem = result.First();
            Assert.IsNotNull(resultItem);
            Assert.AreEqual($"{databaseName}-{tableName}", resultItem.Path);
            Assert.AreEqual($"{databaseName}-{tableName}", resultItem.Content);
        }
    }
}
