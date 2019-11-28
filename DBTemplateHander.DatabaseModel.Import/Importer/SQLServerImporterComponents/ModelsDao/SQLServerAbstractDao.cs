using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao
{
    public abstract class SQLServerAbstractDao<T>
    {
        public List<T> GetAll(SqlConnection openedSqlConnection)
        {
            List<T> result = new List<T>();
            using (SqlCommand command = new SqlCommand(SelectQuery, openedSqlConnection))
            {
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var model = ToModel(dataReader);
                    result.Add(model);
                }
                dataReader.Close();
            }
            return result;
        }

        public abstract string SelectQuery {get;}

        protected abstract T ToModel(SqlDataReader dataReader);
        
    }
}
