using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerIndexesDao : SQLServerAbstractDao<SQLServerIndexesModel>
    {
        public override string SelectQuery => @"select object_id,
name,
index_id,
type,
type_desc,
is_unique,
data_space_id,
ignore_dup_key,
is_primary_key,
is_unique_constraint,
fill_factor,
is_padded,
is_disabled,
is_hypothetical,
is_ignored_in_optimization,
allow_row_locks,
allow_page_locks,
has_filter,
filter_definition,
compression_delay,
suppress_dup_key_messages,
auto_created
from sys.indexes";

        protected override SQLServerIndexesModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerIndexesModel();
            result.object_id = (int)dataReader["object_id"];
            result.name = (string)(dataReader["name"] is DBNull ? null : dataReader["name"]);
            result.index_id = (int)dataReader["index_id"];
            result.type = (byte)dataReader["type"];
            result.type_desc = (string)dataReader["type_desc"];
            result.is_unique = (bool?)(dataReader["is_unique"] is DBNull ? null : dataReader["is_unique"]);
            result.data_space_id = (int?)(dataReader["data_space_id"] is DBNull ? null : dataReader["data_space_id"]);
            result.ignore_dup_key = (bool?)(dataReader["ignore_dup_key"] is DBNull ? null : dataReader["ignore_dup_key"]);
            result.is_primary_key = (bool?)(dataReader["is_primary_key"] is DBNull ? null : dataReader["is_primary_key"]);
            result.is_unique_constraint = (bool?)(dataReader["is_unique_constraint"] is DBNull ? null : dataReader["is_unique_constraint"]);
            result.fill_factor = (byte)(dataReader["fill_factor"]);
            result.is_padded = (bool?)(dataReader["is_padded"] is DBNull ? null : dataReader["is_padded"]);
            result.is_disabled = (bool?)(dataReader["is_disabled"] is DBNull ? null : dataReader["is_disabled"]);
            result.is_hypothetical = (bool?)(dataReader["is_hypothetical"] is DBNull ? null : dataReader["is_hypothetical"]);
            result.is_ignored_in_optimization = (bool?)(dataReader["is_ignored_in_optimization"] is DBNull ? null : dataReader["is_ignored_in_optimization"]);
            result.allow_row_locks = (bool?)(dataReader["allow_row_locks"] is DBNull ? null : dataReader["allow_row_locks"]);
            result.allow_page_locks = (bool?)(dataReader["allow_page_locks"] is DBNull ? null : dataReader["allow_page_locks"]);
            result.has_filter = (bool?)(dataReader["has_filter"] is DBNull ? null : dataReader["has_filter"]);
            result.filter_definition = (string)(dataReader["filter_definition"] is DBNull ? null : dataReader["filter_definition"]);
            result.compression_delay = (int?)(dataReader["compression_delay"] is DBNull ? null : dataReader["compression_delay"]);
            result.suppress_dup_key_messages = (bool?)(dataReader["suppress_dup_key_messages"] is DBNull ? null : dataReader["suppress_dup_key_messages"]);
            result.auto_created = (bool?)(dataReader["auto_created"] is DBNull ? null : dataReader["auto_created"]);
            return result;
        }
    }
}
