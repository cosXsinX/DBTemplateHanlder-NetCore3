using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Utils;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Constraints
{
    public class ForEachTablesForeignVisitContextHanlder : AbstractTableTemplateContextHandler
    {
        public ForEachTablesForeignVisitContextHanlder(ITemplateHandler templateHandler) : base(templateHandler) { }

        public override string StartContext => "{:TDB:CONSTRAINT:CURRENT:VISIT:FOREIGN:TABLE:FOREACH[";

        public override string EndContext => "]::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => throw new NotImplementedException();


        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            var visitedTablesStack = new List<List<ITableModel>>();
            var currentVisitedTables = new List<ITableModel>() { databaseContext.Table };
            while (currentVisitedTables.Any())
            {
                visitedTablesStack = visitedTablesStack.Append(currentVisitedTables).ToList();
                var extractionResults = ExtractForeignTables(databaseContext.Database, currentVisitedTables);
                currentVisitedTables = extractionResults.SelectMany(m => m.Item2).ToList();
            }

            var tableAndDepth = visitedTablesStack.SelectMany(
                (currentList, depth) => currentList.Select(current => Tuple.Create(current, depth))).Skip(1).Reverse().ToList();

            string trimedStringContext = TrimContextFromContextWrapper(StringContext);
            var result = string.Join(string.Empty, tableAndDepth.Select(tableAndDepth => 
                TemplateHandler.HandleTemplate(trimedStringContext,
                    DatabaseContextCopier.CopyWithOverride(databaseContext,tableAndDepth.Item1))));
            return result;
        }

        private IList<ITableModel> ExtractForeignTables(IDatabaseModel databaseModel,ITableModel tableModel)
        {

            var tableKeys = tableModel.
                ForeignKeyConstraints.SelectMany(constraint => constraint.Elements.Select(m => new { m.Foreign.TableName, m.Foreign.SchemaName })).Distinct().ToList();
            var foreignTables = databaseModel.Tables.InnerJoin(tableKeys, m => $"{m.Name}-{m.Schema}", m => $"{m.TableName}-{m.SchemaName}")
                .Select(m => m.Item1).ToList();
            return foreignTables;
        }

        private IList<Tuple<ITableModel, IList<ITableModel>>> ExtractForeignTables(IDatabaseModel databaseModel, IList<ITableModel> tableModels)
        {
            var result = tableModels.Select(table => Tuple.Create(table, ExtractForeignTables(databaseModel, table))).ToList();
            return result;
        }


        protected override void ControlContext(string StringContext, IDatabaseContext databaseContext)
        {
            base.ControlContext(StringContext, databaseContext);
            if (databaseContext.Database == null) throw new ArgumentNullException(nameof(databaseContext.Database));
            if (databaseContext.Database.Tables == null) throw new ArgumentNullException(nameof(databaseContext.Database.Tables));
        } 
    }
}
