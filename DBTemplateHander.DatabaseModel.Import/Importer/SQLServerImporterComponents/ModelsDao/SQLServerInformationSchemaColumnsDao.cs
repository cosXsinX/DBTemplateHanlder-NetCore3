using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerInformationSchemaColumnsDao: SQLServerAbstractDao<SQLServerInformationSchemaColumnsModel>
    {
        public override string SelectQuery => @"SELECT 
TABLE_CATALOG,
TABLE_SCHEMA,
TABLE_NAME,
COLUMN_NAME,
ORDINAL_POSITION,
COLUMN_DEFAULT,
IS_NULLABLE,
DATA_TYPE,
CHARACTER_MAXIMUM_LENGTH,
CHARACTER_OCTET_LENGTH,
NUMERIC_PRECISION,
NUMERIC_PRECISION_RADIX,
NUMERIC_SCALE,
DATETIME_PRECISION,
CHARACTER_SET_CATALOG,
CHARACTER_SET_SCHEMA,
CHARACTER_SET_NAME,
COLLATION_CATALOG,
COLLATION_NAME,
DOMAIN_CATALOG,
DOMAIN_SCHEMA,
DOMAIN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
";

        protected override SQLServerInformationSchemaColumnsModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerInformationSchemaColumnsModel();
            result.TABLE_CATALOG = (string)dataReader["TABLE_CATALOG"];
            result.TABLE_SCHEMA = (string)dataReader["TABLE_SCHEMA"];
            result.TABLE_NAME = (string)dataReader["TABLE_NAME"];
            result.COLUMN_NAME = (string)dataReader["COLUMN_NAME"];
            result.ORDINAL_POSITION = (int)dataReader["ORDINAL_POSITION"];
            result.COLUMN_DEFAULT = (string)(dataReader["COLUMN_DEFAULT"] is DBNull ? null : dataReader["COLUMN_DEFAULT"]);
            result.IS_NULLABLE = (string)dataReader["IS_NULLABLE"];
            result.DATA_TYPE = (string)dataReader["DATA_TYPE"];
            result.CHARACTER_MAXIMUM_LENGTH = (int?)(dataReader["CHARACTER_MAXIMUM_LENGTH"] is DBNull ? null : dataReader["CHARACTER_MAXIMUM_LENGTH"]);
            result.CHARACTER_OCTET_LENGTH = (int?)(dataReader["CHARACTER_OCTET_LENGTH"] is DBNull ? null : dataReader["CHARACTER_OCTET_LENGTH"]);
            result.NUMERIC_PRECISION = (byte?)(dataReader["NUMERIC_PRECISION"] is DBNull ? null : dataReader["NUMERIC_PRECISION"]);
            result.NUMERIC_PRECISION_RADIX = (short?)(dataReader["NUMERIC_PRECISION_RADIX"] is DBNull ? null : dataReader["NUMERIC_PRECISION_RADIX"]);
            result.NUMERIC_SCALE = (int?)(dataReader["NUMERIC_SCALE"] is DBNull ? null : dataReader["NUMERIC_SCALE"]);
            result.DATETIME_PRECISION = (short?)(dataReader["DATETIME_PRECISION"] is DBNull ? null : dataReader["DATETIME_PRECISION"]);
            result.CHARACTER_SET_CATALOG = (string)(dataReader["CHARACTER_SET_CATALOG"] is DBNull ? null : dataReader["CHARACTER_SET_CATALOG"]);
            result.CHARACTER_SET_SCHEMA = (string)(dataReader["CHARACTER_SET_SCHEMA"] is DBNull ? null : dataReader["CHARACTER_SET_SCHEMA"]);
            result.CHARACTER_SET_NAME = (string)(dataReader["CHARACTER_SET_NAME"] is DBNull ? null : dataReader["CHARACTER_SET_NAME"]);
            result.COLLATION_CATALOG = (string)(dataReader["COLLATION_CATALOG"] is DBNull ? null : dataReader["COLLATION_CATALOG"]);
            result.COLLATION_NAME = (string)(dataReader["COLLATION_NAME"] is DBNull ? null : dataReader["COLLATION_NAME"]);
            result.DOMAIN_CATALOG = (string)(dataReader["DOMAIN_CATALOG"] is DBNull ? null : dataReader["DOMAIN_CATALOG"]);
            result.DOMAIN_SCHEMA = (string)(dataReader["DOMAIN_SCHEMA"] is DBNull ? null : dataReader["DOMAIN_SCHEMA"]);
            result.DOMAIN_NAME = (string)(dataReader["DOMAIN_NAME"] is DBNull ? null : dataReader["DOMAIN_NAME"]);
            return result;
        }
    }
}
