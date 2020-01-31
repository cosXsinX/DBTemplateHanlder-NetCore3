using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class ColumnValueConvertTypeColumnContextHandlerUnitTests
    {
        private ColumnValueConvertTypeColumnContextHandler _tested;
        private ColumnValueTypeSemanticDefinition columnValueTypeColumnContextHandler;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            columnValueTypeColumnContextHandler = new ColumnValueTypeSemanticDefinition();
            var typeMappings = new List<ITypeMapping>()
            {
                new TypeMappingForTest()
                {
                    DestinationTypeSetName = "JAVA",
                    TypeMappingItems = new List<ITypeMappingItem>()
                    {
                        new TypeMappingItemForTest(){SourceType="INT",DestinationType="int"},
                        new TypeMappingItemForTest(){SourceType="BIGINT",DestinationType="long"},
                        new TypeMappingItemForTest(){SourceType="BOOLEAN",DestinationType="boolean"},
                        new TypeMappingItemForTest(){SourceType="CHAR",DestinationType="char"},
                        new TypeMappingItemForTest(){SourceType="DATE",DestinationType="Date"},
                        new TypeMappingItemForTest(){SourceType="DATETIME",DestinationType="Date"},
                        new TypeMappingItemForTest(){SourceType="DECIMAL",DestinationType="double"},
                        new TypeMappingItemForTest(){SourceType="INTEGER",DestinationType="int"},
                        new TypeMappingItemForTest(){SourceType="NUMERIC",DestinationType="double"},
                        new TypeMappingItemForTest(){SourceType="REAL",DestinationType="double"},
                        new TypeMappingItemForTest(){SourceType="STRING",DestinationType="String"},
                        new TypeMappingItemForTest(){SourceType="TEXT",DestinationType="String"},
                        new TypeMappingItemForTest(){SourceType="TIME",DestinationType="Date"},
                        new TypeMappingItemForTest(){SourceType="VARCHAR",DestinationType="String"},
                    }
                },
                new TypeMappingForTest()
                {
                    DestinationTypeSetName = "JAVA_WITH_PROCESSED_OUTPUT_TYPE",
                    TypeMappingItems = new List<ITypeMappingItem>()
                    {
                        new TypeMappingItemForTest(){SourceType="INT",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->int"},
                        new TypeMappingItemForTest(){SourceType="BIGINT",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->long"},
                        new TypeMappingItemForTest(){SourceType="BOOLEAN",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->boolean"},
                        new TypeMappingItemForTest(){SourceType="CHAR",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->char"},
                        new TypeMappingItemForTest(){SourceType="DATE",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->Date"},
                        new TypeMappingItemForTest(){SourceType="DATETIME",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->Date"},
                        new TypeMappingItemForTest(){SourceType="DECIMAL",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->double"},
                        new TypeMappingItemForTest(){SourceType="INTEGER",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->int"},
                        new TypeMappingItemForTest(){SourceType="NUMERIC",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->double"},
                        new TypeMappingItemForTest(){SourceType="REAL",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->double"},
                        new TypeMappingItemForTest(){SourceType="STRING",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->String"},
                        new TypeMappingItemForTest(){SourceType="TEXT",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->String"},
                        new TypeMappingItemForTest(){SourceType="TIME",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->Date"},
                        new TypeMappingItemForTest(){SourceType="VARCHAR",DestinationType=$"{columnValueTypeColumnContextHandler.Signature}->String"},
                    }
                },
            };
            _tested = new ColumnValueConvertTypeColumnContextHandler(new Core.TemplateHandlers.Handlers.TemplateHandlerNew(typeMappings),typeMappings); //Bad check how to make full IOC
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenColumnModelIsNull()
        {
            string StringContext = "nianiania";
            _tested.ColumnModel = null;
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"The { nameof(_tested.ColumnModel)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenInternalContextIsEmpty()
        {
            string StringContext = $"{_tested.StartContext}{_tested.EndContext}";
            _tested.ColumnModel = new ColumnModel();
            Assert.Throws<Exception>(() => _tested.processContext(StringContext), $"There is a problem with the function provided in template '{StringContext}' -> The value parameter cannot be empty");
        }

        [TestCase("INT", "JAVA", "int")]
        [TestCase("BIGINT", "JAVA", "long")]
        [TestCase("BOOLEAN", "JAVA", "boolean")]
        [TestCase("CHAR", "JAVA", "char")]
        [TestCase("DATE", "JAVA", "Date")]
        [TestCase("DATETIME", "JAVA", "Date")]
        [TestCase("DECIMAL", "JAVA", "double")]
        [TestCase("INTEGER", "JAVA", "int")]
        [TestCase("INT", "JAVA", "int")]
        [TestCase("NUMERIC", "JAVA", "double")]
        [TestCase("REAL", "JAVA", "double")]
        [TestCase("STRING", "JAVA", "String")]
        [TestCase("TEXT", "JAVA", "String")]
        [TestCase("TIME", "JAVA", "Date")]
        [TestCase("VARCHAR", "JAVA", "String")]
        [TestCase("BLOB", "JAVA", "BLOB")]
        [TestCase("BLOB", "OTHER_UNKNOWN", "CONVERT:UNKNOWN(OTHER_UNKNOWN)")]
        public void ShouldReturnAccurateMappedValue(string columnType,string destinationTypeSet, string expectedOutput)
        {
            _tested.ColumnModel = new ColumnModel() { Type = columnType, };
            var result = _tested.processContext($"{_tested.StartContext}{destinationTypeSet}{_tested.EndContext}");
            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase("INT", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "INT->int")]
        [TestCase("BIGINT", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "BIGINT->long")]
        [TestCase("BOOLEAN", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "BOOLEAN->boolean")]
        [TestCase("CHAR", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "CHAR->char")]
        [TestCase("DATE", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "DATE->Date")]
        [TestCase("DATETIME", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "DATETIME->Date")]
        [TestCase("DECIMAL", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "DECIMAL->double")]
        [TestCase("INTEGER", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "INTEGER->int")]
        [TestCase("INT", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "INT->int")]
        [TestCase("NUMERIC", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "NUMERIC->double")]
        [TestCase("REAL", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "REAL->double")]
        [TestCase("STRING", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "STRING->String")]
        [TestCase("TEXT", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "TEXT->String")]
        [TestCase("TIME", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "TIME->Date")]
        [TestCase("VARCHAR", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "VARCHAR->String")]
        [TestCase("BLOB", "JAVA_WITH_PROCESSED_OUTPUT_TYPE", "BLOB")]
        [TestCase("BLOB", "OTHER_UNKNOWN", "CONVERT:UNKNOWN(OTHER_UNKNOWN)")]
        public void ShouldReturnAccurateAndProcessedMappedValue(string columnType, string destinationTypeSet, string expectedOutput)
        {
            _tested.ColumnModel = new ColumnModel() { Type = columnType, };
            var result = _tested.processContext($"{_tested.StartContext}{destinationTypeSet}{_tested.EndContext}");
            Assert.AreEqual(expectedOutput, result);
        }

        public class TypeMappingForTest : ITypeMapping
        {
            public string DestinationTypeSetName { get; set; }
            public string SourceTypeSetName { get; set; }
            public IList<ITypeMappingItem> TypeMappingItems { get; set; }
        }

        public class TypeMappingItemForTest : ITypeMappingItem
        {
            public string DestinationType { get; set; }
            public string SourceType { get; set; }
        }


    }
}
