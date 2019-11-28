using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerSysKeyConstraintDao : SQLServerAbstractDao<SQLServerSysKeyConstraintModel>
    {
        public override string SelectQuery => @"select 
name,
object_id,
principal_id,
schema_id,
parent_object_id,
type,
type_desc,
create_date,
modify_date,
is_ms_shipped,
is_published,
is_schema_published,
unique_index_id,
is_system_named,
is_enforced as is_enforced
From sys.key_constraints";

        protected override SQLServerSysKeyConstraintModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerSysKeyConstraintModel();
            result.name = (string)dataReader["name"];
            result.object_id = (string)dataReader["object_id"];
            result.principal_id = (int?)(dataReader["principal_id"] is DBNull ? null : dataReader["principal_id"]);
            result.schema_id = (int)dataReader["schema_id"];
            result.parent_object_id = (int)dataReader["parent_object_id"];
            result.type = (string)dataReader["type"];
            result.type_desc = (string)dataReader["type_desc"];
            result.create_date = (DateTime)dataReader["type_desc"];
            result.modify_date = (DateTime?)(dataReader["modify_date"] is DBNull ? null : dataReader["modify_date"]);
            result.is_ms_shipped = (bool)dataReader["is_ms_shipped"];
            result.is_published = (bool)dataReader["is_published"];
            result.is_schema_published = (bool)dataReader["is_schema_published"];
            result.unique_index_id = (int)dataReader["unique_index_id"];
            result.is_system_named = (bool)dataReader["is_system_named"];
            result.is_enforced = (bool)dataReader["is_enforced"];
            return result;
        }
    }
}
