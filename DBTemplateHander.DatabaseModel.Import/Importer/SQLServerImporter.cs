﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        private readonly SQLServerSchemasDao sqlServerSchemasDao = new SQLServerSchemasDao();



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
                indexColumnsModels.InnerJoin(indexesModels.Where(m => m.is_primary_key ?? false), m => $"{m.object_id}-{m.index_id}", m => $"{m.object_id}-{m.index_id}")
                .Select(m => new { indexColumn = m.Item1, index = m.Item2}).ToList();


            IList<SqlModelJointure> sqlModelsWithIndexes =
                databaseAndTableModelAndSchemaAndColumnModel.LeftJoin(indexColumnsAndIndexesSqlModels, m => $"{m.column.object_id}-{m.column.column_id}", m => $"{m.index.object_id}-{m.indexColumn.column_id}").
                Select(m => new SqlModelJointure() { database = m.Item1.database, table = m.Item1.table, schema = m.Item1.schema, 
                    column = m.Item1.column, index = m.Item2?.index, indexColumn = m.Item2?.indexColumn }).ToList();
            ;

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
            var result = new ImporterTableModel();
            result.Name = sqlTableAndSchemaAndColumns.Item1.name;
            result.Schema = sqlTableAndSchemaAndColumns.Item2.name;
            result.Columns = sqlTableAndSchemaAndColumns.Item3.Select(ToColumnModel)
                .GroupBy(m => $"{m.Name}-{m.IsPrimaryKey}-{m.IsAutoGeneratedValue}-{m.IsNotNull}-{m.Type}-{m.ValueMaxSize}",m => m)
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
            result.Type = converted.column.system_type_name??converted.column.user_type_name;
            result.ValueMaxSize = converted.column.max_length;
            return result;
        }


        


        public class ImportedDatabaseModel : IDatabaseModel
        {
            public string Name { get;set; }
            public IList<ITableModel> Tables { get;set; }
            public string TypeSetName { get; set; }
            public string ConnectionString { get; set; }
        }

        public class ImporterTableModel : ITableModel
        {
            public IList<IColumnModel> Columns { get;set; }
            public string Name { get;set; }
            public string Schema { get; set; }
            public IDatabaseModel ParentDatabase { get;set; }
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
        }
    }
}
