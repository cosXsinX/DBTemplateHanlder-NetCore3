using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerIndexColumnsDao : SQLServerAbstractDao<SQLServerIndexColumnsModel>
    {
        public override string SelectQuery => @"select 
object_id,
index_id,
index_column_id,
column_id,
key_ordinal,
partition_ordinal,
is_descending_key,
is_included_column
from sys.index_columns";

        protected override SQLServerIndexColumnsModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerIndexColumnsModel();
            result.object_id = (int)dataReader["object_id"];
            result.index_id = (int)dataReader["index_id"];
            result.index_column_id = (int)dataReader["index_column_id"];
            result.column_id = (int)dataReader["column_id"];
            result.key_ordinal = (short)dataReader["key_ordinal"];
            result.partition_ordinal = (short)dataReader["partition_ordinal"];
            result.is_descending_key = (bool)dataReader["is_descending_key"];
            result.is_included_column = (bool)dataReader["is_included_column"];
            return result;
        }
    }
}
