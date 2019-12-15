using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Utils;

namespace DBTemplateHander.DatabaseModel.Import.Importer
{
    public class SQLServerImporter : IImporter
    {
        private readonly SQLServerDatabaseDao sqlServerDatabaseDao = new SQLServerDatabaseDao();
        private readonly SQLServerTableDao sqlServerTableDao = new SQLServerTableDao();
        private readonly SQLServerColumnDao sqlServerColummnDao = new SQLServerColumnDao();
        private readonly SQLServerInformationSchemaConstraintColumnUsageDao sqlServerInformationSchemaConstraintColumnUsageDao = new SQLServerInformationSchemaConstraintColumnUsageDao();
        private readonly SQLServerSysKeyConstraintDao sqlServerSysKeyConstraintDao = new SQLServerSysKeyConstraintDao();
        private readonly SQLServerIndexesDao sqlServerIndexesDao = new SQLServerIndexesDao();
        private readonly SQLServerIndexColumnsDao sqlServerIndexColumnsDao = new SQLServerIndexColumnsDao();

        

        public string ManagedDbSystem => "Sql Server 2016";

        public IDatabaseModel Import(string connectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            var databaseModels = sqlServerDatabaseDao.GetAll(sqlConnection);
            var tableModels = sqlServerTableDao.GetAll(sqlConnection);
            var columnModels = sqlServerColummnDao.GetAll(sqlConnection);
            var indexColumnsModels = sqlServerIndexColumnsDao.GetAll(sqlConnection);
            var indexesModels = sqlServerIndexesDao.GetAll(sqlConnection);
            sqlConnection.Close();


            var databaseAndTableModel = databaseModels
                .LeftJoin(tableModels, m => true, m => true).ToList();

            var databaseAndTableModelAndColumnModel =
                databaseAndTableModel
                .LeftJoin(columnModels, m => m.Item2.object_id, m => m.object_id).ToList();

            var sqlModels =
                databaseAndTableModelAndColumnModel
                .Select(m => new {database = m.Item1.Item1, table = m.Item1.Item2, column = m.Item2 }).ToList();

            var indexColumnsAndIndexesSqlModels =
                indexColumnsModels.InnerJoin(indexesModels.Where(m => m.is_primary_key ?? false), m => m.object_id, m => m.object_id)
                .Select(m => new { indexColumn = m.Item1, index = m.Item2}).ToList();


            IList<SqlModelJointure> sqlModelsWithIndexes =
                sqlModels.LeftJoin(indexColumnsAndIndexesSqlModels, m => $"{m.column.object_id}-{m.column.column_id}", m => $"{m.index.object_id}-{m.indexColumn.column_id}").
                Select(m => new SqlModelJointure() { database = m.Item1.database, table = m.Item1.table, column = m.Item1.column, index = m.Item2?.index, indexColumn = m.Item2?.indexColumn }).ToList();
            ;

            IDatabaseModel databaseModel = ToDatabaseModel(sqlModelsWithIndexes);
            databaseModel.TypeSetName = ManagedDbSystem;
            return databaseModel;
        }

        public class SqlModelJointure
        {
            public SQLServerDatabaseModel database { get; set; }
            public SQLServerTableModel table { get; set; }
            public SQLServerColumnModel column { get; set; }
            public SQLServerIndexColumnsModel indexColumn { get; set; }
            public SQLServerIndexesModel index { get; set; }
        }

        public IDatabaseModel ToDatabaseModel(IList<SqlModelJointure> sqlModels)
        {
            var SqlServerTableModels = sqlModels
                .GroupBy(m => m.table.object_id)
                .Select(m => Tuple.Create(m.First().table,m.ToList())).ToList();
            var sqlServerDatabaseModel = sqlModels.FirstOrDefault()?.database;
            var result = ToDatabaseModel(sqlServerDatabaseModel, SqlServerTableModels);
            return result;
        }


        public IDatabaseModel ToDatabaseModel(SQLServerDatabaseModel sqlDatabaseModel, IList<Tuple<SQLServerTableModel, List<SqlModelJointure>>> sqlTableAndColumnsTuples)
        {
            if (sqlDatabaseModel == null) return null;
            var result = new ImportedDatabaseModel();
            result.Name = sqlDatabaseModel.database_name??"Unknown Database";
            result.Tables = sqlTableAndColumnsTuples.Select(ToTableModel).ToList();
            return result;
        }

        public ITableModel ToTableModel(Tuple<SQLServerTableModel,List<SqlModelJointure>> sqlTableAndColumns)
        {
            var result = new ImporterTableModel();
            result.Name = sqlTableAndColumns.Item1.name;
            result.Columns = sqlTableAndColumns.Item2.Select(ToColumnModel)
                .GroupBy(m => $"{m.Name}-{m.IsPrimaryKey}-{m.IsAutoGeneratedValue}-{m.IsNotNull}-{m.Type}",m => m)
                .Select(m => m.First())
                .ToList();
            return result;
        }

        public string ToDbTemplateType(string SQLServerType)
        {
            return null;
        }

        public IColumnModel ToColumnModel(SqlModelJointure converted)
        {
            var result = new ImporterColumnModel();
            result.Name = converted.column.name;
            result.IsPrimaryKey = converted?.index?.is_primary_key ?? false;
            result.IsAutoGeneratedValue = converted.column.is_identity; //not sure about the mapping
            result.IsNotNull = !converted.column.is_nullable;
            result.Type = converted.column.system_type_name;
            return result;
        }


        


        public class ImportedDatabaseModel : IDatabaseModel
        {
            public string Name { get;set; }
            public IList<ITableModel> Tables { get;set; }
            public string TypeSetName { get; set; }
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
