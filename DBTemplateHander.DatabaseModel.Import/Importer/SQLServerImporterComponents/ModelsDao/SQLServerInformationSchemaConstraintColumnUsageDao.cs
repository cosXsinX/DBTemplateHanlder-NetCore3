using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerInformationSchemaConstraintColumnUsageDao : SQLServerAbstractDao<SQLServerInformationSchemaConstraintColumnUsageModel>
    {
        public override string SelectQuery => @"Select 
TABLE_CATALOG,
TABLE_SCHEMA,
TABLE_NAME,
COLUMN_NAME,
CONSTRAINT_CATALOG,
CONSTRAINT_SCHEMA,
CONSTRAINT_NAME
From INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE";

        protected override SQLServerInformationSchemaConstraintColumnUsageModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerInformationSchemaConstraintColumnUsageModel();
            result.TABLE_CATALOG = (string)dataReader["TABLE_CATALOG"];
            result.TABLE_SCHEMA = (string)dataReader["TABLE_SCHEMA"];
            result.TABLE_NAME = (string)dataReader["TABLE_NAME"];
            result.COLUMN_NAME = (string)dataReader["COLUMN_NAME"];
            result.CONSTRAINT_CATALOG = (string)dataReader["CONSTRAINT_CATALOG"];
            result.CONSTRAINT_SCHEMA = (string)dataReader["CONSTRAINT_SCHEMA"];
            result.CONSTRAINT_NAME = (string)dataReader["CONSTRAINT_NAME"];
            return result;
        }
    }
}
