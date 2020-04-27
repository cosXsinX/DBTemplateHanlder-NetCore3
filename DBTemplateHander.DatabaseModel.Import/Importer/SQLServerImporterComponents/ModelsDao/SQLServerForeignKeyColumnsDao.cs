using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerForeignKeyColumnsDao : SQLServerAbstractDao<SQLServerForeignKeyColumnsModel>
    {
        public override string SelectQuery => @"Select 
constraint_object_id,
constraint_column_id,
parent_object_id,
parent_column_id,
referenced_object_id,
referenced_column_id 
from sys.foreign_key_columns";

        protected override SQLServerForeignKeyColumnsModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerForeignKeyColumnsModel();
            result.constraint_object_id = (int)dataReader["constraint_object_id"];
            result.constraint_column_id = (int)dataReader["constraint_column_id"];
            result.parent_object_id = (int)dataReader["parent_object_id"];
            result.parent_column_id = (int)dataReader["parent_column_id"];
            result.referenced_object_id = (int)dataReader["referenced_object_id"];
            result.referenced_column_id = (int)dataReader["referenced_column_id"];
            return result;
        }
    }
}
