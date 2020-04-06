using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models;
using DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.ModelsDao;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Service.Contracts.Database;
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
        private readonly SQLServerSchemasDao sqlServerSchemasDao = new SQLServerSchemasDao();

        private readonly SQLServerInformationSchemaColumnsDao sqlServerInformationSchemaColumnsDao = new SQLServerInformationSchemaColumnsDao();



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
            var schemasModels = sqlServerSchemasDao.GetAll(sqlConnection);

            var informationSchemaColumnModels = sqlServerInformationSchemaColumnsDao.GetAll(sqlConnection);
            sqlConnection.Close();


            var databaseAndTableModel = databaseModels
                .LeftJoin(tableModels, m => true, m => true)
                .Select(m => new { database = m.Item1 , table = m.Item2}).ToList();

            var databaseAndTableModelAndSchema = databaseAndTableModel
                .LeftJoin(schemasModels, m => $"{m.table.schema_id}", m => $"{m.schema_id}")
                .Select(m => new { database = m.Item1.database, table = m.Item1.table, schema = m.Item2})
                .ToList();

            var databaseAndTableModelAndSchemaAndColumnModel =
                databaseAndTableModelAndSchema
                .LeftJoin(columnModels, m => m.table.object_id, m => m.object_id)
                .Select(m => new { database = m.Item1.database, table = m.Item1.table, schema = m.Item1.schema, column = m.Item2 }).ToList();

            var indexColumnsAndIndexesSqlModels =
                indexColumnsModels.InnerJoin(indexesModels, m => $"{m.object_id}-{m.index_id}", m => $"{m.object_id}-{m.index_id}")
                .Select(m => new { indexColumn = m.Item1, index = m.Item2}).ToList();


            IList<SqlModelJointure> sqlModelsWithIndexes =
                databaseAndTableModelAndSchemaAndColumnModel.LeftJoin(indexColumnsAndIndexesSqlModels, m => $"{m.column.object_id}-{m.column.column_id}", m => $"{m.index.object_id}-{m.indexColumn.column_id}").
                Select(m => new SqlModelJointure() { database = m.Item1.database, table = m.Item1.table, schema = m.Item1.schema, 
                    column = m.Item1.column, index = m.Item2?.index, indexColumn = m.Item2?.indexColumn }).ToList();

            var sqlModelJointureWithColumnSchemaInformation =
                sqlModelsWithIndexes.LeftJoin(informationSchemaColumnModels, m => $"{m?.database?.database_name}-{m?.schema?.name}-{m?.table?.name}-{m?.column?.name}", m => $"{m.TABLE_CATALOG}-{m.TABLE_SCHEMA}-{m.TABLE_NAME}-{m.COLUMN_NAME}").ToList();

            sqlModelJointureWithColumnSchemaInformation.ForEach(m => m.Item1.informationSchemaColumns = m.Item2);

            IDatabaseModel databaseModel = ToDatabaseModel(connectionString,sqlModelsWithIndexes);
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
            public SQLServerSchemasModel schema { get; set; }
            public SQLServerInformationSchemaColumnsModel informationSchemaColumns { get; set; }
        }

        public IDatabaseModel ToDatabaseModel(string connectionString, IList<SqlModelJointure> sqlModels)
        {
            var SqlServerTableModels = sqlModels
                .GroupBy(m => m.table.object_id)
                .Select(m => {
                    var first = m.First();
                    return Tuple.Create(first.table, first.schema, m.ToList());
                }).ToList();
            var sqlServerDatabaseModel = sqlModels.FirstOrDefault()?.database;
            var result = ToDatabaseModel(connectionString, sqlServerDatabaseModel, SqlServerTableModels);
            return result;
        }


        public IDatabaseModel ToDatabaseModel(string connectionString, 
            SQLServerDatabaseModel sqlDatabaseModel, IList<Tuple<SQLServerTableModel,SQLServerSchemasModel, List<SqlModelJointure>>> sqlTableAndColumnsTuples)
        {
            if (sqlDatabaseModel == null) return null;
            var result = new ImportedDatabaseModel();
            result.Name = sqlDatabaseModel.database_name??"Unknown Database";
            result.ConnectionString = connectionString??"Unknown Connection String";
            result.Tables = sqlTableAndColumnsTuples.Select(ToTableModel).ToList();
            return result;
        }

        public ITableModel ToTableModel(Tuple<SQLServerTableModel,SQLServerSchemasModel,List<SqlModelJointure>> sqlTableAndSchemaAndColumns)
        {
            var result = new ImportedTableModel();
            result.Name = sqlTableAndSchemaAndColumns.Item1.name;
            result.Schema = sqlTableAndSchemaAndColumns.Item2.name;
            result.Columns = sqlTableAndSchemaAndColumns.Item3
                .GroupBy(m => $"{m?.database?.database_name}-{m?.schema?.name}-{m?.table?.name}-{m?.column?.name}")
                .Select(m => Tuple.Create(m.First().column,m.First().schema,m.First().informationSchemaColumns,m.Where(m => m.index != default).Select(m => m.index).ToList()))
                .Select(ToColumnModel)
                .GroupBy(m => $"{m.Name}-{m.IsPrimaryKey}-{m.IsAutoGeneratedValue}-{m.IsNotNull}-{m.Type}-{m.ValueMaxSize}",m => m)
                .Select(m => m.First())
                .ToList();
            return result;
        }


        public IColumnModel ToColumnModel(Tuple<SQLServerColumnModel,SQLServerSchemasModel,SQLServerInformationSchemaColumnsModel, List<SQLServerIndexesModel>> converted)
        {
            var result = new ImporterColumnModel();
            result.Name = converted.Item1.name;
            result.IsPrimaryKey = converted?.Item4?.Any(m => m.is_primary_key??false) ?? false;
            result.IsAutoGeneratedValue = converted.Item1.is_identity || converted.Item1.is_computed;
            result.IsNotNull = !converted.Item1.is_nullable;
            result.Type = converted.Item1.system_type_name??converted.Item1.user_type_name;
            result.ValueMaxSize = ((converted?.Item3?.CHARACTER_MAXIMUM_LENGTH)??converted?.Item3?.NUMERIC_PRECISION)??0;
            result.IsIndexed = converted?.Item4?.Any()??false;
            return result;
        }


        


        public class ImportedDatabaseModel : IDatabaseModel
        {
            public string Name { get;set; }
            public IList<ITableModel> Tables { get;set; }
            public string TypeSetName { get; set; }
            public string ConnectionString { get; set; }
        }

        public class ImportedTableModel : ITableModel
        {
            public IList<IColumnModel> Columns { get;set; }
            public string Name { get;set; }
            public string Schema { get; set; }
            public IDatabaseModel ParentDatabase { get;set; }
            public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }
        }

        public class ImporterColumnModel : IColumnModel
        {
            public bool IsAutoGeneratedValue { get;set; }
            public bool IsNotNull { get;set; }
            public bool IsPrimaryKey { get;set; }
            public string Name { get;set; }
            public string Type { get;set; }
            public int ValueMaxSize { get; set; }
            public ITableModel ParentTable { get;set; }
            public bool IsIndexed { get; set; }
        }
    }
}
