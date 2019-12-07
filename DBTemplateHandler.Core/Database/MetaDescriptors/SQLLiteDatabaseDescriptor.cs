using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database.MetaDescriptors
{
    public class SQLLiteDatabaseDescriptor : AbstractDatabaseDescriptor
    {
        private const string BIGINT_COLUMN_TYPE = "BIGINT";
        private const string BLOB_COLUMN_TYPE = "BLOB";
        private const string BOOLEAN_COLUMN_TYPE = "BOOLEAN";
        private const string CHAR_COLUMN_TYPE = "CHAR";
        private const string DATE_COLUMN_TYPE = "DATE";
        private const string DATETIME_COLUMN_TYPE = "DATETIME";
        private const string DECIMAL_COLUMN_TYPE = "DECIMAL";
        private const string DOUBLE_COLUMN_TYPE = "DOUBLE";
        private const string INTEGER_COLUMN_TYPE = "INTEGER";
        private const string INT_COLUMN_TYPE = "INT";
        private const string NUMERIC_COLUMN_TYPE = "NUMERIC";
        private const string REAL_COLUMN_TYPE = "REAL";
        private const string STRING_COLUMN_TYPE = "STRING";
        private const string TEXT_COLUMN_TYPE = "TEXT";
        private const string TIME_COLUMN_TYPE = "TIME";
        private const string VARCHAR_COLUMN_TYPE = "VARCHAR";

        private readonly static string[] _possibleColumnTypes =
            {
            BIGINT_COLUMN_TYPE,
            BLOB_COLUMN_TYPE,
            BOOLEAN_COLUMN_TYPE,
            CHAR_COLUMN_TYPE,
            DATE_COLUMN_TYPE,
            DATETIME_COLUMN_TYPE,
            DECIMAL_COLUMN_TYPE,
            DOUBLE_COLUMN_TYPE,
            INTEGER_COLUMN_TYPE,
            INT_COLUMN_TYPE,
            NUMERIC_COLUMN_TYPE,
            REAL_COLUMN_TYPE,
            STRING_COLUMN_TYPE,
            TEXT_COLUMN_TYPE,
            TIME_COLUMN_TYPE,
            VARCHAR_COLUMN_TYPE};

        public override string[] GetPossibleColumnTypes()
        {
            return _possibleColumnTypes;
        }

        public override string ConvertType(string ConvertedType, string DestinationEnvironment)
        {
            if (ConvertedType == null) return null;
            if (DestinationEnvironment == null) return null;

            DestinationEnvironment = DestinationEnvironment.ToUpper();
            string result = ConvertedType;
            if (DestinationEnvironment.Equals("JAVA"))
            {
                return ConvertTypeToJava(ConvertedType);
            }
            else return $"CONVERT:UNKNOWN({ConvertedType})";
        }

        public override string ConvertTypeToJava(string ConvertedType)
        {
            if (ConvertedType == null) return null;
            ConvertedType = ConvertedType.ToUpper();
            if (ConvertedType.Equals(BIGINT_COLUMN_TYPE)) return "long";
            else if (ConvertedType.Equals(BLOB_COLUMN_TYPE)) return ConvertedType; //TODO Define associated type
            else if (ConvertedType.Equals(BOOLEAN_COLUMN_TYPE)) return "boolean";
            else if (ConvertedType.Equals(CHAR_COLUMN_TYPE)) return "char";
            else if (ConvertedType.Equals(DATE_COLUMN_TYPE)) return "Date";
            else if (ConvertedType.Equals(DATETIME_COLUMN_TYPE)) return "Date";
            else if (ConvertedType.Equals(DECIMAL_COLUMN_TYPE)) return "double";
            else if (ConvertedType.Equals(DOUBLE_COLUMN_TYPE)) return "double";
            else if (ConvertedType.Equals(INTEGER_COLUMN_TYPE)) return "int";
            else if (ConvertedType.Equals(INT_COLUMN_TYPE)) return "int";
            else if (ConvertedType.Equals(NUMERIC_COLUMN_TYPE)) return "double";
            else if (ConvertedType.Equals(REAL_COLUMN_TYPE)) return "double";
            else if (ConvertedType.Equals(STRING_COLUMN_TYPE)) return "String";
            else if (ConvertedType.Equals(TEXT_COLUMN_TYPE)) return "String";
            else if (ConvertedType.Equals(TIME_COLUMN_TYPE)) return "Date";
            else if (ConvertedType.Equals(VARCHAR_COLUMN_TYPE)) return "String";
            else return ConvertedType;
        }
    }
}
