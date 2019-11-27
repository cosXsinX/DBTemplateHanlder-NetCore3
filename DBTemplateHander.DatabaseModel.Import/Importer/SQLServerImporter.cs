using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using DBTemplateHandler.Core.Database;

namespace DBTemplateHander.DatabaseModel.Import.Importer
{
    public class SQLServerImporter : IImporter
    {
        private const string SelectQueryRadical = @"SELECT 
DB_NAME() As database_name,
columns.object_id AS columns_object_id,
columns.name AS columns_name,
columns.column_id AS columns_column_id,
columns.system_type_id AS columns_system_type_id,
columns.user_type_id AS columns_user_type_id,
columns.max_length AS columns_max_length,
columns.precision AS columns_precision,
columns.scale AS columns_scale,
columns.collation_name AS columns_collation_name,
columns.is_nullable AS columns_is_nullable,
columns.is_ansi_padded AS columns_is_ansi_padded,
columns.is_rowguidcol AS columns_is_rowguidcol,
columns.is_identity AS columns_is_identity,
columns.is_computed AS columns_is_computed,
columns.is_filestream AS columns_is_filestream,
columns.is_replicated AS columns_is_replicated,
columns.is_non_sql_subscribed AS columns_is_non_sql_subscribed,
columns.is_merge_published AS columns_is_merge_published,
columns.is_dts_replicated AS columns_is_dts_replicated,
columns.is_xml_document AS columns_is_xml_document,
columns.xml_collection_id AS columns_xml_collection_id,
columns.default_object_id AS columns_default_object_id,
columns.rule_object_id AS columns_rule_object_id,
columns.is_sparse AS columns_is_sparse,
columns.is_column_set AS columns_is_column_set,
columns.generated_always_type AS columns_generated_always_type,
columns.generated_always_type_desc AS columns_generated_always_type_desc,
columns.encryption_type AS columns_encryption_type,
columns.encryption_type_desc AS columns_encryption_type_desc,
columns.encryption_algorithm_name AS columns_encryption_algorithm_name,
columns.column_encryption_key_id AS columns_column_encryption_key_id,
columns.column_encryption_key_database_name AS columns_column_encryption_key_database_name,
columns.is_hidden AS columns_is_hidden,
columns.is_masked AS columns_is_masked,
columns.graph_type AS columns_graph_type,
columns.graph_type_desc AS columns_graph_type_desc,
tables.name AS tables_name,
tables.object_id AS tables_object_id,
tables.principal_id AS tables_principal_id,
tables.schema_id AS tables_schema_id,
tables.parent_object_id AS tables_parent_object_id,
tables.type AS tables_type,
tables.type_desc AS tables_type_desc,
tables.create_date AS tables_create_date,
tables.modify_date AS tables_modify_date,
tables.is_ms_shipped AS tables_is_ms_shipped,
tables.is_published AS tables_is_published,
tables.is_schema_published AS tables_is_schema_published,
tables.lob_data_space_id AS tables_lob_data_space_id,
tables.filestream_data_space_id AS tables_filestream_data_space_id,
tables.max_column_id_used AS tables_max_column_id_used,
tables.lock_on_bulk_load AS tables_lock_on_bulk_load,
tables.uses_ansi_nulls AS tables_uses_ansi_nulls,
tables.is_replicated AS tables_is_replicated,
tables.has_replication_filter AS tables_has_replication_filter,
tables.is_merge_published AS tables_is_merge_published,
tables.is_sync_tran_subscribed AS tables_is_sync_tran_subscribed,
tables.has_unchecked_assembly_data AS tables_has_unchecked_assembly_data,
tables.text_in_row_limit AS tables_text_in_row_limit,
tables.large_value_types_out_of_row AS tables_large_value_types_out_of_row,
tables.is_tracked_by_cdc AS tables_is_tracked_by_cdc,
tables.lock_escalation AS tables_lock_escalation,
tables.lock_escalation_desc AS tables_lock_escalation_desc,
tables.is_filetable AS tables_is_filetable,
tables.is_memory_optimized AS tables_is_memory_optimized,
tables.durability AS tables_durability,
tables.durability_desc AS tables_durability_desc,
tables.temporal_type AS tables_temporal_type,
tables.temporal_type_desc AS tables_temporal_type_desc,
tables.history_table_id AS tables_history_table_id,
tables.is_remote_data_archive_enabled AS tables_is_remote_data_archive_enabled,
tables.is_external AS tables_is_external,
tables.history_retention_period AS tables_history_retention_period,
tables.history_retention_period_unit AS tables_history_retention_period_unit,
tables.history_retention_period_unit_desc AS tables_history_retention_period_unit_desc,
tables.is_node AS tables_is_node,
tables.is_edge AS tables_is_edge
FROM sys.columns columns JOIN sys.tables tables ON columns.object_id = tables.object_id";

        public string ManagedDbSystem => "Sql Server 2016";

        public IDatabaseModel Import(string connectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            IList<Tuple<SQLServerDatabaseModel,SQLServerTableModel, SQLServerColumnModel>> sqlModels = new List<Tuple<SQLServerDatabaseModel, SQLServerTableModel, SQLServerColumnModel>>();
            using (SqlCommand command = new SqlCommand(SelectQueryRadical,sqlConnection))
            {
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var sqlDatabaseModel = ToSqlServerDatabaseModel(dataReader);
                    var sqlTableModel = ToSqlServerTableModel(dataReader);
                    var sqlColumnModel = ToSqlServerColumnModel(dataReader);
                    sqlModels.Add(Tuple.Create(sqlDatabaseModel, sqlTableModel, sqlColumnModel));
                }
            }

            //var sqlServerInformationSchemaContraintColumnUsages = new List<SQLServerInformationSchemaConstraintColumnUsageModel>();
            //using (SqlCommand command = new SqlCommand(SQLServerInformationSchemaConstraintColumnUsageSelectQueryRadical, sqlConnection))
            //{
            //    var dataReader = command.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        var sqlServerInformationSchemaContraintColumnUsage =
            //            ToSQLServerInformationSchemaConstraintColumnUsageModel(dataReader);
            //    }
            //}

            //var sqlServerSysKeyConstraints = new List<SQLServerSysKeyConstraintModel>();
            //using (SqlCommand command = new SqlCommand(SQLServerSysKeyConstraintsSeleceQueryRadical, sqlConnection))
            //{
            //    var dataReader = command.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        var sqlServerSysKeyConstraint =
            //            ToSQLServerSysKeyConstraints(dataReader);
            //    }
            //}

            sqlConnection.Close();
            IDatabaseModel databaseModel = ToDatabaseModel(sqlModels);
            return databaseModel;
        }

        public SQLServerDatabaseModel ToSqlServerDatabaseModel(SqlDataReader dataReader)
        {
            var result = new SQLServerDatabaseModel();
            result.database_name = (string)dataReader["database_name"];
            return result;
        }

        public SQLServerTableModel ToSqlServerTableModel(SqlDataReader dataReader)
        {
            var result = new SQLServerTableModel();
            result.name = (string)dataReader["tables_name"];
            result.object_id = (int)dataReader["tables_object_id"];
            result.principal_id = (int?)(dataReader["tables_principal_id"] is DBNull ? null:dataReader["tables_principal_id"]) ;
            result.schema_id = (int)dataReader["tables_schema_id"];
            result.parent_object_id = (int)dataReader["tables_parent_object_id"];
            result.Type = (string)dataReader["tables_type"];
            result.type_desc = (string)dataReader["tables_type_desc"];
            result.create_date = (DateTime)dataReader["tables_create_date"];
            result.modify_date = (DateTime)dataReader["tables_modify_date"];
            result.is_ms_shipped = (bool)dataReader["tables_is_ms_shipped"];
            result.is_published = (bool)dataReader["tables_is_published"];
            result.is_schema_published = (bool)dataReader["tables_is_schema_published"];
            result.lob_data_space_id = (int)dataReader["tables_lob_data_space_id"];
            result.filestream_data_space_id = (int?)(dataReader["tables_filestream_data_space_id"] is DBNull ? null: dataReader["tables_filestream_data_space_id"]);
            result.max_column_id_used = (int)dataReader["tables_max_column_id_used"];
            result.lock_on_bulk_load = (bool)dataReader["tables_lock_on_bulk_load"];
            result.uses_ansi_nulls = (bool)dataReader["tables_uses_ansi_nulls"];
            result.is_replicated = (bool)dataReader["tables_is_replicated"];
            result.has_replication_filter = (bool)dataReader["tables_has_replication_filter"];
            result.is_merge_published = (bool)dataReader["tables_is_merge_published"];
            result.is_sync_tran_subscribed = (bool)dataReader["tables_is_sync_tran_subscribed"];
            result.has_unchecked_assembly_data = (bool)dataReader["tables_has_unchecked_assembly_data"];
            result.text_in_row_limit = (int)dataReader["tables_text_in_row_limit"];
            result.large_value_types_out_of_row = (bool)dataReader["tables_large_value_types_out_of_row"];
            result.is_tracked_by_cdc = (bool)dataReader["tables_is_tracked_by_cdc"];
            result.lock_escalation = (byte)dataReader["tables_lock_escalation"];
            result.lock_escalation_desc = (string)dataReader["tables_lock_escalation_desc"];
            result.is_filetable = (bool)dataReader["tables_is_filetable"];
            result.durability = (byte)dataReader["tables_durability"];
            result.durability_desc = (string)dataReader["tables_durability_desc"];
            result.is_memory_optimized = (bool)dataReader["tables_is_memory_optimized"];
            result.temporal_type = (byte)dataReader["tables_temporal_type"];
            result.temporal_type_desc = (string)dataReader["tables_temporal_type_desc"];
            result.history_table_id = (int?)(dataReader["tables_history_table_id"] is DBNull ? null : dataReader["tables_history_table_id"]);
            result.is_remote_data_archive_enabled = (bool)dataReader["tables_is_remote_data_archive_enabled"];
            result.is_external = (bool)dataReader["tables_is_external"];
            result.history_retention_period = (int?)(dataReader["tables_history_retention_period"] is DBNull ? null : dataReader["tables_history_retention_period"]);
            result.history_retention_period_unit = (int?)(dataReader["tables_history_retention_period_unit"] is DBNull ? null : dataReader["tables_history_retention_period"]);
            result.history_retention_period_unit_desc = (string)(dataReader["tables_history_retention_period_unit_desc"] is DBNull ? null : dataReader["tables_history_retention_period_unit_desc"]);
            result.is_node = (bool)dataReader["tables_is_node"];
            result.is_edge = (bool)dataReader["tables_is_edge"];
            return result;
        }

        public SQLServerColumnModel ToSqlServerColumnModel(SqlDataReader dataReader)
        {
            var result = new SQLServerColumnModel();
            result.object_id = (int)dataReader["columns_object_id"];
            result.name = (string)dataReader["columns_name"];
            result.column_id = (int)dataReader["columns_column_id"];
            result.system_type_id = (byte)dataReader["columns_system_type_id"];
            result.user_type_id = (int)dataReader["columns_user_type_id"];
            result.max_length = (short)dataReader["columns_max_length"];
            result.precision = (byte)dataReader["columns_precision"];
            result.scale = (byte)dataReader["columns_scale"];
            result.collation_name = (string)(dataReader["columns_collation_name"] is DBNull ? null : dataReader["columns_collation_name"]);
            result.is_nullable = (bool)dataReader["columns_is_nullable"];
            result.is_ansi_padded = (bool)dataReader["columns_is_ansi_padded"];
            result.is_rowguidcol = (bool)dataReader["columns_is_rowguidcol"];
            result.is_identity = (bool)dataReader["columns_is_identity"];
            result.is_computed = (bool)dataReader["columns_is_computed"];
            result.is_filestream = (bool)dataReader["columns_is_filestream"];
            result.is_replicated = (bool)dataReader["columns_is_replicated"];
            result.is_non_sql_subscribed = (bool)dataReader["columns_is_non_sql_subscribed"];
            result.is_merge_published = (bool)dataReader["columns_is_merge_published"];
            result.is_dts_replicated = (bool)dataReader["columns_is_dts_replicated"];
            result.is_xml_document = (bool)dataReader["columns_is_xml_document"];
            result.xml_collection_id = (int)dataReader["columns_xml_collection_id"];
            result.default_object_id = (int)dataReader["columns_default_object_id"];
            result.rule_object_id = (int)dataReader["columns_rule_object_id"];
            result.is_sparse = (bool)dataReader["columns_is_sparse"];
            result.is_column_set = (bool)dataReader["columns_is_column_set"];
            result.generated_always_type = (byte)dataReader["columns_generated_always_type"];
            result.generated_always_type_desc = (string)dataReader["columns_generated_always_type_desc"];
            result.encryption_type = (int?)(dataReader["columns_encryption_type"] is DBNull ? null : dataReader["columns_encryption_type"]);
            result.encryption_type_desc = (string)(dataReader["columns_encryption_type_desc"] is DBNull ? null : dataReader["columns_encryption_type_desc"]);
            result.encryption_algorithm_name = (string)(dataReader["columns_encryption_algorithm_name"] is DBNull ? null : dataReader["columns_encryption_algorithm_name"]);
            result.column_encryption_key_id = (int?)(dataReader["columns_column_encryption_key_id"] is DBNull ? null : dataReader["columns_column_encryption_key_id"]);
            result.column_encryption_key_database_name = (string)(dataReader["columns_column_encryption_key_database_name"] is DBNull ? null : dataReader["columns_column_encryption_key_database_name"]);
            result.is_hidden = (bool)dataReader["columns_is_hidden"];
            result.is_masked = (bool)dataReader["columns_is_masked"];
            return result;
        }

        public IDatabaseModel ToDatabaseModel(IList<Tuple<SQLServerDatabaseModel, SQLServerTableModel,SQLServerColumnModel>> sqlModels)
        {
            var SqlServerTableModels = sqlModels
                .GroupBy(m => m.Item2.object_id)
                .Select(m => Tuple.Create(m.First().Item2,m.Select(j => j.Item3).ToList())).ToList();
            var sqlServerDatabaseModel = sqlModels.FirstOrDefault()?.Item1;
            var result = ToDatabaseModel(sqlServerDatabaseModel,SqlServerTableModels);
            return result;
        }


        public IDatabaseModel ToDatabaseModel(SQLServerDatabaseModel sqlDatabaseModel, IList<Tuple<SQLServerTableModel, List<SQLServerColumnModel>>> sqlTableAndColumnsTuples)
        {
            if (sqlDatabaseModel == null) return null;
            var result = new ImportedDatabaseModel();
            result.Name = sqlDatabaseModel.database_name??"Unknown Database";
            result.Tables = sqlTableAndColumnsTuples.Select(ToTableModel).ToList();
            return result;
        }

        public ITableModel ToTableModel(Tuple<SQLServerTableModel,List<SQLServerColumnModel>> sqlTableAndColumns)
        {
            var result = new ImporterTableModel();
            result.Name = sqlTableAndColumns.Item1.name;
            result.Columns = sqlTableAndColumns.Item2.Select(ToColumnModel).ToList();
            return result;
        }

        public IColumnModel ToColumnModel(SQLServerColumnModel converted)
        {
            var result = new ImporterColumnModel();
            result.Name = converted.name;
            //result.IsPrimaryKey = //TODO => Must get indexes to know if it is a primary key or not
            result.IsAutoGeneratedValue = converted.is_identity; //TODO not sure about the mapping
            result.IsNotNull = !converted.is_nullable;
            return result;
        }

        

        public class SQLServerDatabaseModel
        {
            public string database_name { get; set; }
        }

        

        

        private const string SQLServerInformationSchemaConstraintColumnUsageSelectQueryRadical = @"Select 
is_constraint_column_usage.TABLE_CATALOG as is_constraint_column_usage_TABLE_CATALOG,
is_constraint_column_usage.TABLE_SCHEMA as is_constraint_column_usage_TABLE_SCHEMA,
is_constraint_column_usage.TABLE_NAME as is_constraint_column_usage_TABLE_NAME,
is_constraint_column_usage.COLUMN_NAME as is_constraint_column_usage_COLUMN_NAME,
is_constraint_column_usage.CONSTRAINT_CATALOG as is_constraint_column_usage_CONSTRAINT_CATALOG,
is_constraint_column_usage.CONSTRAINT_SCHEMA as is_constraint_column_usage_CONSTRAINT_SCHEMA,
is_constraint_column_usage.CONSTRAINT_NAME as is_constraint_column_usage_CONSTRAINT_NAME
From INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as is_constraint_column_usage";

        public SQLServerInformationSchemaConstraintColumnUsageModel ToSQLServerInformationSchemaConstraintColumnUsageModel(SqlDataReader dataReader)
        {
            var result = new SQLServerInformationSchemaConstraintColumnUsageModel();
            result.TABLE_CATALOG = (string)dataReader["is_constraint_column_usage_TABLE_CATALOG"];
            result.TABLE_SCHEMA = (string)dataReader["is_constraint_column_usage_TABLE_SCHEMA"];
            result.TABLE_NAME = (string)dataReader["is_constraint_column_usage_TABLE_NAME"];
            result.COLUMN_NAME = (string)dataReader["is_constraint_column_usage_COLUMN_NAME"];
            result.CONSTRAINT_CATALOG = (string)dataReader["is_constraint_column_usage_CONSTRAINT_CATALOG"];
            result.CONSTRAINT_SCHEMA = (string)dataReader["is_constraint_column_usage_CONSTRAINT_SCHEMA"];
            result.CONSTRAINT_NAME = (string)dataReader["is_constraint_column_usage_CONSTRAINT_NAME"];
            return result;
        }

        

        public SQLServerSysKeyConstraintModel ToSQLServerSysKeyConstraints(SqlDataReader dataReader)
        {
            var result = new SQLServerSysKeyConstraintModel();
            result.name = (string)dataReader["sys_key_constraints_name"];
            result.object_id = (string)dataReader["sys_key_constraints_object_id"];
            result.principal_id = (int?)(dataReader["sys_key_constraints_principal_id"] is DBNull ? null : dataReader["sys_key_constraints_principal_id"]) ;
            result.schema_id = (int)dataReader["sys_key_constraints_schema_id"];
            result.parent_object_id = (int)dataReader["sys_key_constraints_parent_object_id"];
            result.type = (string)dataReader["sys_key_constraints_type"];
            result.type_desc = (string)dataReader["sys_key_constraints_type_desc"];
            result.create_date = (DateTime)dataReader["sys_key_constraints_type_desc"];
            result.modify_date = (DateTime?)(dataReader["sys_key_constraints_modify_date"] is DBNull ? null : dataReader["sys_key_constraints_modify_date"]);
            result.is_ms_shipped = (bool)dataReader["sys_key_constraints_is_ms_shipped"];
            result.is_published = (bool)dataReader["sys_key_constraints_is_published"];
            result.is_schema_published = (bool)dataReader["sys_key_constraints_is_schema_published"];
            result.unique_index_id = (int)dataReader["sys_key_constraints_unique_index_id"];
            result.is_system_named = (bool)dataReader["sys_key_constraints_is_system_named"];
            result.is_enforced = (bool)dataReader["sys_key_constraints_is_enforced"];
            return result;
        }

        private const string SQLServerSysKeyConstraintsSeleceQueryRadical = @"select 
sys_key_constraints.name as sys_key_constraints_name,
sys_key_constraints.object_id as sys_key_constraints_object_id,
sys_key_constraints.principal_id as sys_key_constraints_principal_id,
sys_key_constraints.schema_id as sys_key_constraints_schema_id,
sys_key_constraints.parent_object_id as sys_key_constraints_parent_object_id,
sys_key_constraints.type as sys_key_constraints_type,
sys_key_constraints.type_desc as sys_key_constraints_type_desc,
sys_key_constraints.create_date as sys_key_constraints_create_date,
sys_key_constraints.modify_date as sys_key_constraints_modify_date,
sys_key_constraints.is_ms_shipped as sys_key_constraints_is_ms_shipped,
sys_key_constraints.is_published as sys_key_constraints_is_published,
sys_key_constraints.is_schema_published as sys_key_constraints_is_schema_published,
sys_key_constraints.unique_index_id as sys_key_constraints_unique_index_id,
sys_key_constraints.is_system_named as sys_key_constraints_is_system_named,
sys_key_constraints.is_enforced as sys_key_constraints_is_enforced
From sys.key_constraints as sys_key_constraints
";

        


        public class ImportedDatabaseModel : IDatabaseModel
        {
            public string Name { get;set; }
            public IList<ITableModel> Tables { get;set; }
        }

        public class ImporterTableModel : ITableModel
        {
            public IList<IColumnModel> Columns { get;set; }
            public string Name { get;set; }
            public IDatabaseModel ParentDatabase { get;set; }
        }

        public class ImporterColumnModel : IColumnModel
        {
            public bool IsAutoGeneratedValue { get;set; }
            public bool IsNotNull { get;set; }
            public bool IsPrimaryKey { get;set; }
            public string Name { get;set; }
            public string Type { get;set; }
            public ITableModel ParentTable { get;set; }
        }
    }
}
