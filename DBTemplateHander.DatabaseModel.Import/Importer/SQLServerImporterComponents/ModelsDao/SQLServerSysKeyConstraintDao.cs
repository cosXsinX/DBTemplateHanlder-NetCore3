using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public class SQLServerSysKeyConstraintDao : SQLServerAbstractDao<SQLServerSysKeyConstraintModel>
    {
        public override string SelectQuery => throw new NotImplementedException();

        protected override SQLServerSysKeyConstraintModel ToModel(SqlDataReader source)
        {
            throw new NotImplementedException();
        }
    }
}
