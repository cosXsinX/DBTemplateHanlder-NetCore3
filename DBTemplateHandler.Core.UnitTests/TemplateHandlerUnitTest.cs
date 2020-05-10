﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests
{
    [TestFixture]
    public class TemplateHandlerUnitTest
    {	
        private static ITemplateHandler BuildTemplateHandler(IList<ITypeMapping> typeMappings)
        {
            return new TemplateHandler(typeMappings);
        }

	    private const string DATABASE_NAME = "DATABASE_NAME";
        DatabaseModel _databaseDescriptionPojo;
        private void InitDatabaseDescriptionPOJO()
        {
            _databaseDescriptionPojo = new DatabaseModel();
            _databaseDescriptionPojo.Name = DATABASE_NAME;
            IList<ITableModel> tableList = new List<ITableModel>()
            {
                _firstTableDescriptionPOJO,
                _secondTableDescriptionPOJO
            };
            _databaseDescriptionPojo.Tables = tableList;
        }


        private const string FIRST_TABLE_NAME = "FIRST_TABLE_NAME";
        TableModel _firstTableDescriptionPOJO;
        private void InitFirstTableDescriptionPOJO()
        {
            _firstTableDescriptionPOJO = new TableModel(FIRST_TABLE_NAME);
            _firstTableDescriptionPOJO.Name = FIRST_TABLE_NAME;
            _firstTableDescriptionPOJO.Columns = new List<IColumnModel> { _primaryNotAutoColumnDescriptionPOJO, _autoColumnDescriptionPOJO };
        }

        private const String SECOND_TABLE_NAME = "SECOND_TABLE_NAME";
        TableModel _secondTableDescriptionPOJO;
        private void InitSecondTableDescriptionPOJO()
        {
            _secondTableDescriptionPOJO = new TableModel(SECOND_TABLE_NAME);
            _secondTableDescriptionPOJO.Columns = new List<IColumnModel> { _primaryNotAutoColumnDescriptionPOJO, _autoColumnDescriptionPOJO };
        }


        private const String PRIMARY_NOT_AUTO_COLUMN_NAME = "PRIMARY_NOT_AUTO_COLUMN_NAME";
        private const String PRIMARY_NOT_AUTO_SQLLITE_TYPE = "INT";
        ColumnModel _primaryNotAutoColumnDescriptionPOJO;
        private void InitPrimaryNotAutoColumnDescriptionPOJO()
        {
            _primaryNotAutoColumnDescriptionPOJO =
                    new ColumnModel
                        (PRIMARY_NOT_AUTO_COLUMN_NAME, PRIMARY_NOT_AUTO_SQLLITE_TYPE, true);
        }

        private const String AUTO_COLUMN_NAME = "AUTO_COLUMN_NAME";
        private const String AUTO_COLUMN_SQLLITE_TYPE = "INT";
        ColumnModel _autoColumnDescriptionPOJO;
        private void InitAutoColumnDescriptionPOJO()
        {
            _autoColumnDescriptionPOJO =
                    new ColumnModel
                        (AUTO_COLUMN_NAME, AUTO_COLUMN_SQLLITE_TYPE, false);
            _autoColumnDescriptionPOJO.IsAutoGeneratedValue = true;
        }

        private const String AUTO_AND_PRIMARY_COLUMN_NAME = "AUTO_AND_PRIMARY_COLUMN_NAME";
        private const String AUTO_AND_PRIMARY_SQLLITE_TYPE = "Int";
        ColumnModel _autoAndPrimaryColumnDescriptionPOJO;
        private void InitAutoAndPrimaryColumnDescriptionPOJO()
        {
            _autoAndPrimaryColumnDescriptionPOJO =
                    new ColumnModel(AUTO_AND_PRIMARY_COLUMN_NAME, AUTO_AND_PRIMARY_SQLLITE_TYPE, true);
            _autoAndPrimaryColumnDescriptionPOJO.IsAutoGeneratedValue = true;
        }

        private const String AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME = "AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN";
        private const String AUTO_AND_NOT_NULL_AND_PRIMARY_SQLLITE_TYPE = "AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN";
        ColumnModel _autoAndNotNullAndPrimaryColumnDescriptionPOJO;
        private void InitAutoAndNotNullAndPrimaryColumnDescriptionPOJO()
        {
            _autoAndNotNullAndPrimaryColumnDescriptionPOJO =
                    new ColumnModel(AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME, AUTO_AND_NOT_NULL_AND_PRIMARY_SQLLITE_TYPE, true);
            _autoAndNotNullAndPrimaryColumnDescriptionPOJO.IsNotNull = true;
            _autoAndNotNullAndPrimaryColumnDescriptionPOJO.IsAutoGeneratedValue = true;
        }

        [SetUp]
        public void TestInitialization()
        {
            //Columns definition Initialization
            InitPrimaryNotAutoColumnDescriptionPOJO();
            InitAutoColumnDescriptionPOJO();
            InitAutoAndPrimaryColumnDescriptionPOJO();
            InitAutoAndNotNullAndPrimaryColumnDescriptionPOJO();

            //Tables definition Initialization
            InitFirstTableDescriptionPOJO();
            InitSecondTableDescriptionPOJO();

            //Database definition Initialization
            InitDatabaseDescriptionPOJO();
        }

        const string NEW_LINE_CHAR = "\t\n";
        public static string GetContextHandlerSignatures<T>(string header) where T : ITemplateContextHandler
        {
            var provider = new TemplateContextHandlerPackageProvider<T>(BuildTemplateHandler(null),null);
            var handlers = provider.GetHandlers();
            var splittedResult =
                Enumerable.Repeat(header, 1)
                .Concat(handlers.Select(m => m.Signature));
            var result = string.Join(NEW_LINE_CHAR, splittedResult);
            return result;
        }

        [Test]
        public void GetAllTemplateContextHandlerSignatureTest()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GetContextHandlerSignatures<AbstractDatabaseTemplateContextHandler>("Database context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractTableTemplateContextHandler>("Table context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractColumnTemplateContextHandler>("Column context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractFunctionTemplateContextHandler>("Function context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            var result = stringBuilder.ToString();
            TestContext.Write(result);
        }


        [Test]
        public void TemplateValidatorTest()
        {
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(BuildTemplateHandler(null));
            var TemplateValidator = new TemplateValidator(BuildTemplateHandler(null),null);
            String submittedString = ColumnNameColumnContextHandler.Signature;
            bool value = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, value);
        }


        [Test]
        public void Success_when_auto_column_name_is_returned()
        {
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(BuildTemplateHandler(null));
            var TemplateHandlerNew = BuildTemplateHandler(null);
            string submittedString = ColumnNameColumnContextHandler.Signature;
            string result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            string expectedResult = AUTO_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_earliest_start_context_is_returned()
        {
            var provider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(BuildTemplateHandler(null),null); //TODO Add Interface Liskov Principle with TemplateHandlerNew
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(BuildTemplateHandler(null));
            var IsColumnNotAutoGeneratedValueColumnContextHandler = new IsColumnNotAutoGeneratedValueColumnContextHandler(BuildTemplateHandler(null));
            String submittedString =  IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            String result = provider.GetHandlerStartContextWordAtEarliestPosition(submittedString);
            String expectedResult = IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_lattest_end_context_is_returned()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var provider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(templateHandlerNew, null); //TODO Add Interface Liskov Principle with TemplateHandlerNew
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var IsColumnNotAutoGeneratedValueColumnContextHandler = new IsColumnNotAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            string submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            string result = provider.GetHandlerEndContextWordAtLatestPosition(submittedString);
            string expectedResult = IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_lattest_start_context_is_returned()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var provider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(BuildTemplateHandler(null), null); //TODO Add Interface Liskov Principle with TemplateHandlerNew
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var IsColumnNotAutoGeneratedValueColumnContextHandler = new IsColumnNotAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            String result = provider.GetHandlerStartContextWordAtLattestPosition(submittedString);
            String expectedResult = ColumnNameColumnContextHandler.StartContext;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_start_context_word_does_not_contains_other_start_context_word()
        {
            var provider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(BuildTemplateHandler(null), null); //TODO Add Interface Liskov Principle with TemplateHandlerNew
            IEnumerable<AbstractTemplateContextHandler> handlers =
                    provider.GetHandlers();
            foreach(AbstractTemplateContextHandler currentHandler in handlers)
            {
                foreach(AbstractTemplateContextHandler secondCurrentHandler in handlers)
                {
                    if (secondCurrentHandler.GetType().Equals(currentHandler.GetType())) continue;
                    if (secondCurrentHandler.StartContext.Contains(currentHandler.StartContext))
                    {

                        TestContext.Write("'" + currentHandler.GetType().Name + "' handler start context word :'" + secondCurrentHandler.StartContext + "' contains '" + currentHandler.GetType().Name + "' context handler start context word '" + currentHandler.StartContext + "'");
                        Assert.Fail();
                        return;
                    }
                }
            }
            Assert.IsTrue(true);
        }

        //Template validation unit test
        [Test]
        public void Success_when_if_not_auto_conditionned_if_nested_name_template_string_is_validated()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var TemplateValidator = new TemplateValidator(BuildTemplateHandler(null),null);
            var IsColumnNotAutoGeneratedValueColumnContextHandler = new IsColumnNotAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            string submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }


        //Template treatment output unit tests
        [Test]
        public void Success_when_no_output_for_auto_string_when_not_auto_is_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var IsColumnNotAutoGeneratedValueColumnContextHandler = new IsColumnNotAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnNotAutoGeneratedValueColumnContextHandler.EndContext;
            var TemplateHandlerNew = BuildTemplateHandler(null);
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            String expectedResult = "";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_string_when_auto_is_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            String submittedString = IsColumnAutoGeneratedValueColumnContextHandler.StartContext + ColumnNameColumnContextHandler.Signature + IsColumnAutoGeneratedValueColumnContextHandler.EndContext;
            var TemplateHandlerNew = BuildTemplateHandler(null);
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            String expectedResult = AUTO_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_validated_when_primary_and_auto_is_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var TemplateValidator = new TemplateValidator(templateHandlerNew, null); //Ugly implement interfaces and IOC
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);

            String submittedString = IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_and_primary_column_when_primary_and_auto_is_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            string submittedString = IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext;
            string result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndPrimaryColumnDescriptionPOJO);
            string expectedResult = AUTO_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_and_not_null_and_primary_column_when_not_null_and_primary_and_auto_is_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var IsColumnNotNullValueColumnContextHandler = new IsColumnNotNullValueColumnContextHandler(templateHandlerNew);
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.StartContext +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext +
                    IsColumnNotNullValueColumnContextHandler.EndContext;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndNotNullAndPrimaryColumnDescriptionPOJO);
            String expectedResult = AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_end_context_count_match_seven()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var IsColumnNotNullValueColumnContextHandler = new IsColumnNotNullValueColumnContextHandler(templateHandlerNew);
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);

            var provider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(BuildTemplateHandler(null), null); //TODO Add Interface Liskov Principle with TemplateHandlerNew
            string submittedString =
                    IsColumnNotNullValueColumnContextHandler.StartContext +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext +
                    IsColumnNotNullValueColumnContextHandler.EndContext +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext;
            int result = provider.CountEndContextWordIn(submittedString);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_validated_when_auto_generated_and_primary_appended_with_auto_generated_is_specified_in_template()
        {

            var templateHandlerNew = BuildTemplateHandler(null);
            var TemplateValidator = new TemplateValidator(templateHandlerNew, null);
            var IsColumnNotNullValueColumnContextHandler = new IsColumnNotNullValueColumnContextHandler(templateHandlerNew);
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.StartContext +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext +
                    IsColumnNotNullValueColumnContextHandler.EndContext +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_two_auto_generated_and_primary_column_name_when_auto_generated_and_primary_appended_with_auto_generated_is_specified_in_template()
        {

            var templateHandlerNew = BuildTemplateHandler(null);
            var IsColumnNotNullValueColumnContextHandler = new IsColumnNotNullValueColumnContextHandler(templateHandlerNew);
            var IsColumnPrimaryKeyColumnContextHandler = new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew);
            var IsColumnAutoGeneratedValueColumnContextHandler = new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew);
            var ColumnNameColumnContextHandler = new ColumnNameColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.StartContext +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext +
                    IsColumnNotNullValueColumnContextHandler.EndContext +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.StartContext +
                    IsColumnAutoGeneratedValueColumnContextHandler.StartContext +
                    ColumnNameColumnContextHandler.Signature +
                    IsColumnAutoGeneratedValueColumnContextHandler.EndContext +
                    IsColumnPrimaryKeyColumnContextHandler.EndContext;

            string result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndNotNullAndPrimaryColumnDescriptionPOJO);
            string expectedResult = AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME + " " + AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_first_table_name_when_is_table_name_word_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var TableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString = TableNameTableContextHandler.Signature;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = FIRST_TABLE_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        

        [Test]
        public void Success_when_output_is_database_all_table_name_when_is_database_foreach_table_name_specified_in_template()
        {
            var templateHandler = BuildTemplateHandler(null);
            var ForEachTableDatabaseContextHandler = new ForEachTableDatabaseContextHandler(templateHandler);
            var TableNameTableContextHandler = new TableNameTableContextHandler(templateHandler);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString = ForEachTableDatabaseContextHandler.StartContext +
                    TableNameTableContextHandler.Signature + "," +
                    ForEachTableDatabaseContextHandler.EndContext;
            String result = TemplateHandlerNew.HandleTemplate(submittedString, _databaseDescriptionPojo, null, null,null);
            String expectedResult = FIRST_TABLE_NAME + "," + SECOND_TABLE_NAME + ",";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_Hello_when_hello_firt_character_to_upper_case_specified_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var FirstLetterToUpperCaseFunctionTemplateHandler = new FirstLetterToUpperCaseFunctionTemplateHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString = FirstLetterToUpperCaseFunctionTemplateHandler.StartContext
                    + "hello" +
                    FirstLetterToUpperCaseFunctionTemplateHandler.EndContext;
            String result = TemplateHandlerNew.HandleTemplate(submittedString, _databaseDescriptionPojo, null, null,null);
            String expectedResult = "Hello";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_one_when_is_first_column_is_specified_and_column_index_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var ForEachColumnTableContextHandler = new ForEachColumnTableContextHandler(templateHandlerNew);
            var IsColumnAFirstColumnContextHandler = new IsColumnAFirstColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString = ForEachColumnTableContextHandler.StartContext +
                    IsColumnAFirstColumnContextHandler.StartContext +
                    "hello" +
                    IsColumnAFirstColumnContextHandler.EndContext +
                    ForEachColumnTableContextHandler.EndContext;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = "hello";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_one_when_is_last_column_is_specified_and_column_index_in_template()
        {
            var templateHandlerNew = BuildTemplateHandler(null);
            var ForEachColumnTableContextHandler = new ForEachColumnTableContextHandler(templateHandlerNew);
            var IsColumnALastColumnContextHandler = new IsColumnALastColumnContextHandler(templateHandlerNew);
            var TemplateHandlerNew = BuildTemplateHandler(null);

            String submittedString = ForEachColumnTableContextHandler.StartContext +
                    IsColumnALastColumnContextHandler.StartContext +
                    "hello" +
                    IsColumnALastColumnContextHandler.EndContext +
                    ForEachColumnTableContextHandler.EndContext;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = "hello";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
