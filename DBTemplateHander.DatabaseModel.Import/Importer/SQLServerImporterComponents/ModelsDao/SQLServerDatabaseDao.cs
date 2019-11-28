using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerDatabaseDao : SQLServerAbstractDao<SQLServerDatabaseModel>
    {
        public override string SelectQuery => @"SELECT 
DB_NAME() As database_name";

        protected override SQLServerDatabaseModel ToModel(SqlDataReader dataReader)
        {
            var result = new SQLServerDatabaseModel();
            result.database_name = (string)dataReader["database_name"];
            return result;
        }
    }
}
