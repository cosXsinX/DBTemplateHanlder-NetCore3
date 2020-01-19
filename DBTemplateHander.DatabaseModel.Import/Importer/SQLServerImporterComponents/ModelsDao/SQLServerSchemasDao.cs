using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerSchemasDao : SQLServerAbstractDao<SQLServerSchemasModel>
    {
        public override string SelectQuery => @"Select name,
schema_id,
principal_id 
FROM sys.schemas";

        protected override SQLServerSchemasModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerSchemasModel();
            result.name = (string)dataReader["name"];
            result.schema_id = (int)dataReader["schema_id"];
            result.principal_id = (int?)(dataReader["principal_id"] is DBNull ? null : dataReader["principal_id"]);
            return result;
        }
    }
}
