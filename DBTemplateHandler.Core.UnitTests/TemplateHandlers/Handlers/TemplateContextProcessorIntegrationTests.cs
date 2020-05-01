using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Handlers
{
    [TestFixture]
    public class TemplateContextProcessorIntegrationTests
    {
        ITemplateHandler _templateHandlerNew;
        TemplateContextProcessor _tested;
        [OneTimeSetUp]
        public void SetUp()
        {
            var typeMappings = new List<ITypeMapping>() { };
            _templateHandlerNew = TemplateHandlerBuilder.Build(typeMappings);
            _tested = new TemplateContextProcessor(_templateHandlerNew, typeMappings);
        }

        [Test]
        public void ProcessTemplateContextCompositeShouldReturnColumnName()
        {
            string expected = "HelloWorldColumn";
            var columnNameContextHandler = new ColumnNameColumnContextHandler(_templateHandlerNew);

            TemplateContextComposite templateContextComposite = new TemplateContextComposite() {
                current = new ColumnTemplateContext()
                {
                    StartIndex = 0,
                    ContextDepth =0,
                    InnerContent = string.Empty,
                    StartContextDelimiter = columnNameContextHandler.StartContext,
                    EndContextDelimiter = columnNameContextHandler.EndContext,
                },
                childs = null,
            };
            var actual = _tested.ProcessTemplateContextComposite(templateContextComposite, new ProcessorDatabaseContext()
            {
                Database = new DatabaseModel(),
                Table = new TableModel(),
                Column = new ColumnModel()
                {
                    Name = expected,
                }
            });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessTemplateContextCompositeShouldReturnTableName()
        {
            string expected = "HelloWorldTable";
            var tableNameContextHandler = new TableNameTableContextHandler(_templateHandlerNew);
            TemplateContextComposite templateContextComposite = new TemplateContextComposite()
            {
                current = new TableTemplateContext()
                {
                    StartIndex = 0,
                    ContextDepth = 0,
                    InnerContent = string.Empty,
                    StartContextDelimiter = tableNameContextHandler.StartContext,
                    EndContextDelimiter = tableNameContextHandler.EndContext,
                },
                childs = null,
            };
            var actual = _tested.ProcessTemplateContextComposite(templateContextComposite, new ProcessorDatabaseContext()
            {
                Database = new DatabaseModel(),
                Table = new TableModel()
                {
                    Name = expected,
                }
            });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessTemplateContextCompositeShouldReturnDatabaseName()
        {
            string expected = "HelloWorldDatabase";
            var databaseNameContextHandler = new DatabaseNameDatabaseContextHandler(_templateHandlerNew);
            TemplateContextComposite templateContextComposite = new TemplateContextComposite()
            {
                current = new DatabaseTemplateContext()
                {
                    StartIndex = 0,
                    ContextDepth = 0,
                    InnerContent = string.Empty,
                    StartContextDelimiter = databaseNameContextHandler.StartContext,
                    EndContextDelimiter = databaseNameContextHandler.EndContext,
                },
                childs = null,
            };
            var actual = _tested.ProcessTemplateContextComposite(templateContextComposite, new ProcessorDatabaseContext()
            {
                Database = new DatabaseModel()
                {
                    Name = expected,
                }
            });
            Assert.AreEqual(expected, actual);
        }
    }
}
