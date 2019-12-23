using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerColumnDao : SQLServerAbstractDao<SQLServerColumnModel>
    {
        public override string SelectQuery => @"SELECT 
object_id,
name,
column_id,
system_type_id,
TYPE_NAME(system_type_id) system_type_name,
user_type_id,
TYPE_NAME(user_type_id) user_type_name,
max_length,
precision,
scale,
collation_name,
is_nullable,
is_ansi_padded,
is_rowguidcol,
is_identity,
is_computed,
is_filestream,
is_replicated,
is_non_sql_subscribed,
is_merge_published,
is_dts_replicated,
is_xml_document,
xml_collection_id,
default_object_id,
rule_object_id,
is_sparse,
is_column_set,
generated_always_type,
generated_always_type_desc,
encryption_type,
encryption_type_desc,
encryption_algorithm_name,
column_encryption_key_id,
column_encryption_key_database_name,
is_hidden,
is_masked,
graph_type,
graph_type_desc
FROM sys.columns
";

        protected override SQLServerColumnModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerColumnModel();
            result.object_id = (int)dataReader["object_id"];
            result.name = (string)dataReader["name"];
            result.column_id = (int)dataReader["column_id"];
            result.system_type_id = (byte)dataReader["system_type_id"];
            result.system_type_name = (string)(dataReader["system_type_name"] is DBNull ? null : dataReader["system_type_name"]) ;
            result.user_type_id = (int)dataReader["user_type_id"];
            result.user_type_name = (string)(dataReader["user_type_name"] is DBNull ? null : dataReader["user_type_name"]);
            result.max_length = (short)dataReader["max_length"];
            result.precision = (byte)dataReader["precision"];
            result.scale = (byte)dataReader["scale"];
            result.collation_name = (string)(dataReader["collation_name"] is DBNull ? null : dataReader["collation_name"]);
            result.is_nullable = (bool)dataReader["is_nullable"];
            result.is_ansi_padded = (bool)dataReader["is_ansi_padded"];
            result.is_rowguidcol = (bool)dataReader["is_rowguidcol"];
            result.is_identity = (bool)dataReader["is_identity"];
            result.is_computed = (bool)dataReader["is_computed"];
            result.is_filestream = (bool)dataReader["is_filestream"];
            result.is_replicated = (bool)dataReader["is_replicated"];
            result.is_non_sql_subscribed = (bool)dataReader["is_non_sql_subscribed"];
            result.is_merge_published = (bool)dataReader["is_merge_published"];
            result.is_dts_replicated = (bool)dataReader["is_dts_replicated"];
            result.is_xml_document = (bool)dataReader["is_xml_document"];
            result.xml_collection_id = (int)dataReader["xml_collection_id"];
            result.default_object_id = (int)dataReader["default_object_id"];
            result.rule_object_id = (int)dataReader["rule_object_id"];
            result.is_sparse = (bool)dataReader["is_sparse"];
            result.is_column_set = (bool)dataReader["is_column_set"];
            result.generated_always_type = (byte)dataReader["generated_always_type"];
            result.generated_always_type_desc = (string)dataReader["generated_always_type_desc"];
            result.encryption_type = (int?)(dataReader["encryption_type"] is DBNull ? null : dataReader["encryption_type"]);
            result.encryption_type_desc = (string)(dataReader["encryption_type_desc"] is DBNull ? null : dataReader["encryption_type_desc"]);
            result.encryption_algorithm_name = (string)(dataReader["encryption_algorithm_name"] is DBNull ? null : dataReader["encryption_algorithm_name"]);
            result.column_encryption_key_id = (int?)(dataReader["column_encryption_key_id"] is DBNull ? null : dataReader["column_encryption_key_id"]);
            result.column_encryption_key_database_name = (string)(dataReader["column_encryption_key_database_name"] is DBNull ? null : dataReader["column_encryption_key_database_name"]);
            result.is_hidden = (bool)dataReader["is_hidden"];
            result.is_masked = (bool)dataReader["is_masked"];
            return result;
        }
    }
}
