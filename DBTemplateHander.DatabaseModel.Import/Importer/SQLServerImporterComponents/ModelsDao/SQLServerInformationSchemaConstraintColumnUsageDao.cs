using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerInformationSchemaConstraintColumnUsageDao : SQLServerAbstractDao<SQLServerInformationSchemaConstraintColumnUsageModel>
    {
        public override string SelectQuery => throw new NotImplementedException();

        protected override SQLServerInformationSchemaConstraintColumnUsageModel ToModel(SqlDataReader source)
        {
            throw new NotImplementedException();
        }
    }
}
