using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerTableDao : SQLServerAbstractDao<SQLServerTableModel>
    {
        public override string SelectQuery => @"SELECT name ,
object_id ,
principal_id ,
schema_id ,
parent_object_id ,
type ,
type_desc ,
create_date ,
modify_date ,
is_ms_shipped ,
is_published ,
is_schema_published ,
lob_data_space_id ,
filestream_data_space_id ,
max_column_id_used ,
lock_on_bulk_load ,
uses_ansi_nulls ,
is_replicated ,
has_replication_filter ,
is_merge_published ,
is_sync_tran_subscribed ,
has_unchecked_assembly_data ,
text_in_row_limit ,
large_value_types_out_of_row ,
is_tracked_by_cdc ,
lock_escalation ,
lock_escalation_desc ,
is_filetable ,
is_memory_optimized ,
durability ,
durability_desc ,
temporal_type ,
temporal_type_desc ,
history_table_id ,
is_remote_data_archive_enabled ,
is_external ,
history_retention_period ,
history_retention_period_unit ,
history_retention_period_unit_desc ,
is_node ,
is_edge
FROM sys.tables";

        protected override SQLServerTableModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerTableModel();
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
            result.lob_data_space_id = (int)dataReader["lob_data_space_id"];
            result.filestream_data_space_id = (int?)(dataReader["filestream_data_space_id"] is DBNull ? null : dataReader["filestream_data_space_id"]);
            result.max_column_id_used = (int)dataReader["max_column_id_used"];
            result.lock_on_bulk_load = (bool)dataReader["lock_on_bulk_load"];
            result.uses_ansi_nulls = (bool)dataReader["uses_ansi_nulls"];
            result.is_replicated = (bool)dataReader["is_replicated"];
            result.has_replication_filter = (bool)dataReader["has_replication_filter"];
            result.is_merge_published = (bool)dataReader["is_merge_published"];
            result.is_sync_tran_subscribed = (bool)dataReader["is_sync_tran_subscribed"];
            result.has_unchecked_assembly_data = (bool)dataReader["has_unchecked_assembly_data"];
            result.text_in_row_limit = (int)dataReader["text_in_row_limit"];
            result.large_value_types_out_of_row = (bool)dataReader["large_value_types_out_of_row"];
            result.is_tracked_by_cdc = (bool)dataReader["is_tracked_by_cdc"];
            result.lock_escalation = (byte)dataReader["lock_escalation"];
            result.lock_escalation_desc = (string)dataReader["lock_escalation_desc"];
            result.is_filetable = (bool)dataReader["is_filetable"];
            result.durability = (byte)dataReader["durability"];
            result.durability_desc = (string)dataReader["durability_desc"];
            result.is_memory_optimized = (bool)dataReader["is_memory_optimized"];
            result.temporal_type = (byte)dataReader["temporal_type"];
            result.temporal_type_desc = (string)dataReader["temporal_type_desc"];
            result.history_table_id = (int?)(dataReader["history_table_id"] is DBNull ? null : dataReader["history_table_id"]);
            result.is_remote_data_archive_enabled = (bool)dataReader["is_remote_data_archive_enabled"];
            result.is_external = (bool)dataReader["is_external"];
            result.history_retention_period = (int?)(dataReader["history_retention_period"] is DBNull ? null : dataReader["history_retention_period"]);
            result.history_retention_period_unit = (int?)(dataReader["history_retention_period_unit"] is DBNull ? null : dataReader["history_retention_period"]);
            result.history_retention_period_unit_desc = (string)(dataReader["history_retention_period_unit_desc"] is DBNull ? null : dataReader["history_retention_period_unit_desc"]);
            result.is_node = (bool)dataReader["is_node"];
            result.is_edge = (bool)dataReader["is_edge"];
            return result;
        }
    }
}
