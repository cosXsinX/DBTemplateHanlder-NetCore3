﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Database.MetaDescriptors;
using DBTemplateHandler.Core.TemplateHandlers;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests
{
    public class TemplateHandlerUnitTest
    {	
	    private const String DATABASE_NAME = "DATABASE_NAME";
        DatabaseDescriptor _databaseDescriptionPojo;
        private void InitDatabaseDescriptionPOJO()
        {
            _databaseDescriptionPojo = new DatabaseDescriptor();
            _databaseDescriptionPojo.Name = DATABASE_NAME;
            List<TableDescriptor> tableList = new List<TableDescriptor>()
            {
                _firstTableDescriptionPOJO,
                _secondTableDescriptionPOJO
            };
            _databaseDescriptionPojo.Tables = tableList;
        }


        private const string FIRST_TABLE_NAME = "FIRST_TABLE_NAME";
        TableDescriptor _firstTableDescriptionPOJO;
        private void InitFirstTableDescriptionPOJO()
        {
            _firstTableDescriptionPOJO = new TableDescriptor(FIRST_TABLE_NAME);
            _firstTableDescriptionPOJO.Name = FIRST_TABLE_NAME;
            _firstTableDescriptionPOJO.Columns.Add(_primaryNotAutoColumnDescriptionPOJO);
            _firstTableDescriptionPOJO.Columns.Add(_autoColumnDescriptionPOJO);
        }

        private const String SECOND_TABLE_NAME = "SECOND_TABLE_NAME";
        TableDescriptor _secondTableDescriptionPOJO;
        private void InitSecondTableDescriptionPOJO()
        {
            _secondTableDescriptionPOJO = new TableDescriptor(SECOND_TABLE_NAME);
            _secondTableDescriptionPOJO.Columns.Add(_primaryNotAutoColumnDescriptionPOJO);
            _secondTableDescriptionPOJO.Columns.Add(_autoColumnDescriptionPOJO);
        }


        private const String PRIMARY_NOT_AUTO_COLUMN_NAME = "PRIMARY_NOT_AUTO_COLUMN_NAME";
        private const String PRIMARY_NOT_AUTO_SQLLITE_TYPE = "INT";
        ColumnDescriptor _primaryNotAutoColumnDescriptionPOJO;
        private void InitPrimaryNotAutoColumnDescriptionPOJO()
        {
            _primaryNotAutoColumnDescriptionPOJO =
                    new ColumnDescriptor
                        (PRIMARY_NOT_AUTO_COLUMN_NAME, PRIMARY_NOT_AUTO_SQLLITE_TYPE, true);
        }

        private const String AUTO_COLUMN_NAME = "AUTO_COLUMN_NAME";
        private const String AUTO_COLUMN_SQLLITE_TYPE = "INT";
        ColumnDescriptor _autoColumnDescriptionPOJO;
        private void InitAutoColumnDescriptionPOJO()
        {
            _autoColumnDescriptionPOJO =
                    new ColumnDescriptor
                        (AUTO_COLUMN_NAME, AUTO_COLUMN_SQLLITE_TYPE, false);
            _autoColumnDescriptionPOJO.IsAutoGeneratedValue = true;
        }

        private const String AUTO_AND_PRIMARY_COLUMN_NAME = "AUTO_AND_PRIMARY_COLUMN_NAME";
        private const String AUTO_AND_PRIMARY_SQLLITE_TYPE = "Int";
        ColumnDescriptor _autoAndPrimaryColumnDescriptionPOJO;
        private void InitAutoAndPrimaryColumnDescriptionPOJO()
        {
            _autoAndPrimaryColumnDescriptionPOJO =
                    new ColumnDescriptor(AUTO_AND_PRIMARY_COLUMN_NAME, AUTO_AND_PRIMARY_SQLLITE_TYPE, true);
            _autoAndPrimaryColumnDescriptionPOJO.IsAutoGeneratedValue = true;
        }

        private const String AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME = "AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN";
        private const String AUTO_AND_NOT_NULL_AND_PRIMARY_SQLLITE_TYPE = "AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN";
        ColumnDescriptor _autoAndNotNullAndPrimaryColumnDescriptionPOJO;
        private void InitAutoAndNotNullAndPrimaryColumnDescriptionPOJO()
        {
            _autoAndNotNullAndPrimaryColumnDescriptionPOJO =
                    new ColumnDescriptor(AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME, AUTO_AND_NOT_NULL_AND_PRIMARY_SQLLITE_TYPE, true);
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

        [Test]
        public void FirstTableColumnsAsJavaTableConstantsTest()
        {
            StringBuilder submittedTemplateStringBuilder = new StringBuilder();
            submittedTemplateStringBuilder.Append("public class ColumnTableTemplateJavaTestTemplate {\n");
            submittedTemplateStringBuilder.Append("\tpublic final static String TABLE_NAME = \"" + TemplateSemanticReferenceClass.TEMPLATE_TABLE_WORD + "\";\n");
            submittedTemplateStringBuilder.Append("\t" + TemplateSemanticReferenceClass.TEMPLATE_FOREACH_COLUMN_START_CONTEXT + "\n");
            submittedTemplateStringBuilder.Append("\tpublic final static String " + TemplateSemanticReferenceClass.TEMPLATE_FOREACH_CURRENT_COLUMN_WORD + "_COLUMN = \"" + TemplateSemanticReferenceClass.TEMPLATE_FOREACH_CURRENT_COLUMN_WORD + "\";\n");
            submittedTemplateStringBuilder.Append("\t" + TemplateSemanticReferenceClass.TEMPLATE_FOREACH_COLUMN_END_CONTEXT + "\n");
            submittedTemplateStringBuilder.Append("}\n");

            AbstractDatabaseDescriptor descriptor = new SQLLiteDatabaseDescriptor();
            TableTemplateHandler handler = TableTemplateHandler.TableDescriptionPOJOToTableTemplateHandler(_firstTableDescriptionPOJO, descriptor);
            String outputString = handler.generateOutputStringFromTemplateString(submittedTemplateStringBuilder.ToString(), out var errors);

            String expectedOutput = "public class ColumnTableTemplateJavaTestTemplate {\n" +
            "\tpublic final static String TABLE_NAME = \"FIRST_TABLE_NAME\";\n" +
            "\t\n" +
            "\tpublic final static String PRIMARY_NOT_AUTO_COLUMN_NAME_COLUMN = \"PRIMARY_NOT_AUTO_COLUMN_NAME\";\n" +
            "\t\n" +
            "\tpublic final static String AUTO_COLUMN_NAME_COLUMN = \"AUTO_COLUMN_NAME\";\n" +
            "\t\n" +
            "}\n";

            Assert.AreEqual(expectedOutput, outputString);
        }


        [Test]
        public void GetAllTemplateContextHandlerSignatureTest()
        {
            String result = TemplateContextHandlerPackageProvider.getAllContextHandlerSignature();
            TestContext.Write(result);
        }

        [Test]
        public void Success_when_default_provided_context_handlers_are_the_same_as_original_provided_context_handlers()
        {
            List<String> columnContextHandlersDiff =
                    TemplateContextHandlerPackageProvider.GetDefaultLoadAndOriginalLoadColumnContextHandlerDifferenceSignatureArray();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (String current in columnContextHandlersDiff)
            {
                stringBuilder.Append("The column context handler '" + current + "' was added but not added to the default load methods TemplateContextHandlerPackageProvider.DefaultLoadColumnContextHandlerDefault\n");
            }
            List<String> tableContextHandlersDiff =
                    TemplateContextHandlerPackageProvider.GetDefaultLoadAndOriginalLoadTableContextHandlerDifferenceSignatureArray();
            foreach (String current in tableContextHandlersDiff)
            {
                stringBuilder.Append("The table context handler '" + current + "' was added but not added to the default load methods TemplateContextHandlerPackageProvider.DefaultLoadTableContextHandlerDefault\n");
            }
            List<String> databaseContextHandlersDiff =
                    TemplateContextHandlerPackageProvider.GetDefaultLoadAndOriginalLoadDatabaseContextHandlerDifferenceSignatureArray();
            foreach (String current in databaseContextHandlersDiff)
            {
                stringBuilder.Append("The database context handler '" + current + "' was added but not added to the default load methods TemplateContextHandlerPackageProvider.DefaultLoadTableContextHandlerDefault\n");
            }

            List<String> functionContextHandlersDiff =
                    TemplateContextHandlerPackageProvider.GetDefaultLoadAndOriginalLoadFunctionContextHandlerDifferenceSignatureArray();
            foreach (String current in functionContextHandlersDiff)
            {
                stringBuilder.Append("The function context handler '" + current + "' was added but not added to the default load methods TemplateContextHandlerPackageProvider.DefaultLoadTableContextHandlerDefault\n");
            }
            bool isDifference = false;
            isDifference = isDifference || (columnContextHandlersDiff.Count > 0);
            isDifference = isDifference || (tableContextHandlersDiff.Count > 0);
            isDifference = isDifference || (databaseContextHandlersDiff.Count > 0);
            isDifference = isDifference || (functionContextHandlersDiff.Count > 0);
            if (isDifference) Assert.Fail(stringBuilder.ToString());
            Assert.IsTrue(true);
        }

        [Test]
        public void TestTemplateHandlerNew_getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence()
        {
            String submittedString = "Hello I am the best and I am the most wonderful";
            String searchedWord = "am";
            String result = StringUtilities.
                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(submittedString, searchedWord);
            String expectedResult = "Hello I ";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestTemplateHandlerNew_getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence()
        {
            String submittedString = "Hello I am the best and I am the most wonderful";
            String searchedWord = "am";
            String result = StringUtilities.
                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(submittedString, searchedWord);
            String expectedResult = " the best and I am the most wonderful";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TemplateValidatorTest()
        {
            String submittedString = ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD;
            bool value = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, value);
        }


        [Test]
        public void Success_when_auto_column_name_is_returned()
        {
            String submittedString = ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            String expectedResult = AUTO_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_earliest_start_context_is_returned()
        {
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            String result = TemplateContextHandlerPackageProvider.getHandlerStartContextWordAtEarliestPositionInSubmittedString(submittedString);
            String expectedResult = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_lattest_end_context_is_returned()
        {
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            String result = TemplateContextHandlerPackageProvider.getHandlerEndContextWordAtLatestPositionInSubmittedString(submittedString);
            String expectedResult = IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_lattest_start_context_is_returned()
        {
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            String result = TemplateContextHandlerPackageProvider.getHandlerStartContextWordAtLattestPositionInSubmittedString(submittedString);
            String expectedResult = ColumnNameColumnContextHandler.START_CONTEXT_WORD;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_start_context_word_does_not_contains_other_start_context_word()
        {
            IEnumerable<AbstractTemplateContextHandler> handlers =
                    TemplateContextHandlerPackageProvider.getAllContextHandler();
            foreach(AbstractTemplateContextHandler currentHandler in handlers)
            {
                foreach(AbstractTemplateContextHandler secondCurrentHandler in handlers)
                {
                    if (secondCurrentHandler.GetType().Equals(currentHandler.GetType())) continue;
                    if (secondCurrentHandler.getStartContextStringWrapper().Contains(currentHandler.getStartContextStringWrapper()))
                    {

                        TestContext.Write("'" + currentHandler.GetType().Name + "' handler start context word :'" + secondCurrentHandler.getStartContextStringWrapper() + "' contains '" + currentHandler.GetType().Name + "' context handler start context word '" + currentHandler.getStartContextStringWrapper() + "'");
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
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }


        //Template treatment output unit tests
        [Test]
        public void Success_when_no_output_for_auto_string_when_not_auto_is_specified_in_template()
        {
            String submittedString = IsColumnNotAutoGeneratedValueColumnContextHandler2.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnNotAutoGeneratedValueColumnContextHandler2.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            String expectedResult = "";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_string_when_auto_is_specified_in_template()
        {
            String submittedString = IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD + ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoColumnDescriptionPOJO);
            String expectedResult = AUTO_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_validated_when_primary_and_auto_is_specified_in_template()
        {
            String submittedString = IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_and_primary_column_when_primary_and_auto_is_specified_in_template()
        {
            String submittedString = IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndPrimaryColumnDescriptionPOJO);
            String expectedResult = AUTO_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_for_auto_and_not_null_and_primary_column_when_not_null_and_primary_and_auto_is_specified_in_template()
        {
            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnNotNullValueColumnContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndNotNullAndPrimaryColumnDescriptionPOJO);
            String expectedResult = AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_end_context_count_match_seven()
        {
            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnNotNullValueColumnContextHandler.END_CONTEXT_WORD +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD;
            int result = TemplateContextHandlerPackageProvider.countEndContextWordInSubmittedString(submittedString);
            int expectedResult = 7;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_is_validated_when_auto_generated_and_primary_appended_with_auto_generated_is_specified_in_template()
        {
            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnNotNullValueColumnContextHandler.END_CONTEXT_WORD +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD;
            bool result = TemplateValidator.TemplateStringValidation(submittedString);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_two_auto_generated_and_primary_column_name_when_auto_generated_and_primary_appended_with_auto_generated_is_specified_in_template()
        {
            String submittedString =
                    IsColumnNotNullValueColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnNotNullValueColumnContextHandler.END_CONTEXT_WORD +
                    " " +
                    IsColumnPrimaryKeyColumnContextHandler.START_CONTEXT_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD +
                    IsColumnAutoGeneratedValueColumnContextHandler.END_CONTEXT_WORD +
                    IsColumnPrimaryKeyColumnContextHandler.END_CONTEXT_WORD;

            String result = TemplateHandlerNew.HandleTableColumnTemplate(submittedString, _autoAndNotNullAndPrimaryColumnDescriptionPOJO);
            String expectedResult = AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME + " " + AUTO_AND_NOT_NULL_AND_PRIMARY_COLUMN_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_first_table_name_when_is_table_name_word_specified_in_template()
        {
            String submittedString = TableNameTableContextHandler.TEMPLATE_TABLE_WORD;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = FIRST_TABLE_NAME;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_first_table_all_column_name_when_is_table_foreach_column_name_specified_in_template()
        {
            String submittedString = ForEachColumnTableContextHandler.START_CONTEXT_WORD +
                    ColumnNameColumnContextHandler.TEMPLATE_TABLE_WORD + "," +
                    ForEachColumnTableContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = PRIMARY_NOT_AUTO_COLUMN_NAME + "," + AUTO_COLUMN_NAME + ",";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_database_all_table_name_when_is_database_foreach_table_name_specified_in_template()
        {
            String submittedString = ForEachTableDatabaseContextHandler.START_CONTEXT_WORD +
                    TableNameTableContextHandler.TEMPLATE_TABLE_WORD + "," +
                    ForEachTableDatabaseContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTemplate(submittedString, _databaseDescriptionPojo, null, null);
            String expectedResult = FIRST_TABLE_NAME + "," + SECOND_TABLE_NAME + ",";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_Hello_when_hello_firt_character_to_upper_case_specified_in_template()
        {
            String submittedString = FirstLetterToUpperCaseFunctionTemplateHandler.START_CONTEXT_WORD
                    + "hello" +
                    FirstLetterToUpperCaseFunctionTemplateHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTemplate(submittedString, _databaseDescriptionPojo, null, null);
            String expectedResult = "Hello";
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void Success_when_output_is_one_when_is_first_column_is_specified_and_column_index_in_template()
        {
            String submittedString = ForEachColumnTableContextHandler.START_CONTEXT_WORD +
                    IsColumnAFirstColumnContextHandler.START_CONTEXT_WORD +
                    "hello" +
                    IsColumnAFirstColumnContextHandler.END_CONTEXT_WORD +
                    ForEachColumnTableContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = "hello";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Success_when_output_is_one_when_is_last_column_is_specified_and_column_index_in_template()
        {
            String submittedString = ForEachColumnTableContextHandler.START_CONTEXT_WORD +
                    IsColumnALastColumnContextHandler.START_CONTEXT_WORD +
                    "hello" +
                    IsColumnALastColumnContextHandler.END_CONTEXT_WORD +
                    ForEachColumnTableContextHandler.END_CONTEXT_WORD;
            String result = TemplateHandlerNew.HandleTableTemplate(submittedString, _firstTableDescriptionPOJO);
            String expectedResult = "hello";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
