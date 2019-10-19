using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database.MetaDescriptors
{
    public class SQLLiteDatabaseDescriptor : AbstractDatabaseDescriptor
    {
        private const String BIGINT_COLUMN_TYPE = "BIGINT";
        private const String BLOB_COLUMN_TYPE = "BLOB";
        private const String BOOLEAN_COLUMN_TYPE = "BOOLEAN";
        private const String CHAR_COLUMN_TYPE = "CHAR";
        private const String DATE_COLUMN_TYPE = "DATE";
        private const String DATETIME_COLUMN_TYPE = "DATETIME";
        private const String DECIMAL_COLUMN_TYPE = "DECIMAL";
        private const String DOUBLE_COLUMN_TYPE = "DOUBLE";
        private const String INTEGER_COLUMN_TYPE = "INTEGER";
        private const String INT_COLUMN_TYPE = "INT";
        private const String NUMERIC_COLUMN_TYPE = "NUMERIC";
        private const String REAL_COLUMN_TYPE = "REAL";
        private const String STRING_COLUMN_TYPE = "STRING";
        private const String TEXT_COLUMN_TYPE = "TEXT";
        private const String TIME_COLUMN_TYPE = "TIME";
        private const String VARCHAR_COLUMN_TYPE = "VARCHAR";

        private readonly static String[] _possibleColumnTypes =
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

        public override String[] get_possibleColumnTypes()
        {
            return _possibleColumnTypes;
        }

        public override String ConvertType(String ConvertedType, String DestinationEnvironment)
        {
            if (ConvertedType == null) return null;
            if (DestinationEnvironment == null) return null;

            DestinationEnvironment = DestinationEnvironment.ToUpper();
            String result = ConvertedType;
            if (DestinationEnvironment.Equals("JAVA"))
            {
                return ConvertTypeToJava(ConvertedType);
            }
            else return $"CONVERT:UNKNOWN({ConvertedType})";
        }

        public override String ConvertTypeToJava(String ConvertedType)
        {
            if (ConvertedType == null) return null;
            ConvertedType = ConvertedType.ToUpper();
            if (ConvertedType.Equals(BIGINT_COLUMN_TYPE)) return "long";
            else if (ConvertedType.Equals(BLOB_COLUMN_TYPE)) return ConvertedType; //TODO Define associated type
            else if (ConvertedType.Equals(BOOLEAN_COLUMN_TYPE)) return "boolean";
            else if (ConvertedType.Equals(CHAR_COLUMN_TYPE)) return "char";
            else if (ConvertedType.Equals(DATE_COLUMN_TYPE)) return "Date";
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
