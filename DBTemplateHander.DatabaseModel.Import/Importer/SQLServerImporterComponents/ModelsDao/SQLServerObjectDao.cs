using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerObjectDao : SQLServerAbstractDao<SQLServerObjectModel>
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
is_schema_published
from sys.objects";

        protected override SQLServerObjectModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerObjectModel();
            result.name = (string)dataReader["name"];
            result.object_id = (int)dataReader["object_id"];
            result.principal_id = (int?)(dataReader["principal_id"] is DBNull?null: dataReader["principal_id"]);
            result.schema_id = (int)dataReader["schema_id"];
            result.parent_object_id = (int)dataReader["parent_object_id"];
            result.type = (string)dataReader["type"];
            result.type_desc = (string)dataReader["type_desc"];
            result.create_date = (DateTime)dataReader["create_date"];
            result.modify_date = (DateTime)dataReader["modify_date"];
            result.is_ms_shipped = (bool)dataReader["is_ms_shipped"];
            result.is_published = (bool)dataReader["is_published"];
            result.is_schema_published = (bool)dataReader["is_schema_published"];
            return result;
        }
    }
}
