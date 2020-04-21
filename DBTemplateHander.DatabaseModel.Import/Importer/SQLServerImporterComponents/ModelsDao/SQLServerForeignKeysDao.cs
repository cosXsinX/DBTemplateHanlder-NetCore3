using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerForeignKeysDao : SQLServerAbstractDao<SQLServerForeignKeysModel>
    {
        public override string SelectQuery => @"select name,
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
referenced_object_id,
key_index_id,
is_disabled,
is_not_for_replication,
is_not_trusted,
delete_referential_action,
delete_referential_action_desc,
update_referential_action,
update_referential_action_desc,
is_system_named
from sys.foreign_keys";

        protected override SQLServerForeignKeysModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerForeignKeysModel();
            result.name = (string)dataReader["name"];
            result.object_id = (int)dataReader["object_id"];
            result.principal_id = (int?)(dataReader["principal_id"] is DBNull ? null : dataReader["principal_id"]);
            result.schema_id = (int)dataReader["schema_id"];
            result.parent_object_id = (int)dataReader["parent_object_id"];
            result.type = (string)dataReader["type"];
            result.type_desc = (string)dataReader["type_desc"];
            result.create_date = (DateTime)dataReader["create_date"];
            result.modify_date = (DateTime)dataReader["modify_date"];
            result.is_ms_shipped = (bool)dataReader["is_ms_shipped"];
            result.is_published = (bool)dataReader["is_published"];
            result.is_schema_published = (bool)dataReader["is_schema_published"];
            //Foreign key specific columns
            result.referenced_object_id = (int)dataReader["referenced_object_id"];
            result.key_index_id = (int)dataReader["key_index_id"];
            result.is_disabled = (bool)dataReader["is_disabled"];
            result.is_not_for_replication = (bool)dataReader["is_not_for_replication"];
            result.is_not_trusted = (bool)dataReader["is_not_trusted"];
            result.delete_referential_action = (byte)dataReader["delete_referential_action"];
            result.delete_referential_action_desc = (string)dataReader["delete_referential_action_desc"];
            result.update_referential_action = (byte)dataReader["update_referential_action"];
            result.update_referential_action_desc = (string)dataReader["update_referential_action_desc"];
            result.is_system_named = (bool)dataReader["is_system_named"];
            return result;

        }
    }
}
