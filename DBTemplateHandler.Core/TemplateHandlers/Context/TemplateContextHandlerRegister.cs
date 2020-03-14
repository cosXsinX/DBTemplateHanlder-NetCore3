﻿using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public class TemplateContextHandlerRegister
    {
        private readonly IList<ITemplateContextHandler> _contextHandlers = new List<ITemplateContextHandler>(){ };

        public TemplateContextHandlerRegister(TemplateHandlerNew templateHandlerNew, IList<ITypeMapping> typeMappings)
        {
            _contextHandlers =
            new List<ITemplateContextHandler>()
            {
                //Preprocessors contextes
                new MappingDeclarePreprcessorContextHandler(templateHandlerNew),
                //Column contextes
                new AutoColumnIndexColumnContextHandler(templateHandlerNew),
                new ColumnIndexColumnContextHandler(templateHandlerNew),
                new ColumnNameColumnContextHandler(templateHandlerNew),
                new ColumnValueConvertTypeColumnContextHandler(templateHandlerNew,typeMappings),
                new ColumnValueTypeColumnContextHandler(templateHandlerNew),
                new ColumnValueMaxSizeColumnContextHandler(templateHandlerNew),
                new IsAutoColumnAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsAutoColumnALastAutoColumnContextHandler(templateHandlerNew),
                new IsAutoColumnNotAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsAutoColumnNotALastAutoColumnContextHandler(templateHandlerNew),
                new IsColumnAFirstColumnContextHandler(templateHandlerNew),
                new IsColumnALastColumnContextHandler(templateHandlerNew),
                new IsColumnAutoGeneratedValueColumnContextHandler(templateHandlerNew),
                new IsColumnNotAFirstColumnContextHandler(templateHandlerNew),
                new IsColumnNotALastColumnContextHandler(templateHandlerNew),
                new IsColumnNotAutoGeneratedValueColumnContextHandler(templateHandlerNew),
                new IsColumnNotNullValueColumnContextHandler(templateHandlerNew),
                new IsColumnNullValueColumnContextHandler(templateHandlerNew),
                new IsColumnNotPrimaryKeyColumnContextHandler(templateHandlerNew),
                new IsColumnPrimaryKeyColumnContextHandler(templateHandlerNew),
                new IsNotAutoColumnAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsNotAutoColumnALastAutoColumnContextHandler(templateHandlerNew),
                new IsNotAutoColumnNotAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsNotAutoColumnNotALastAutoColumnContextHandler(templateHandlerNew),
                new IsNotNullColumnAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsNotNullColumnALastAutoColumnContextHandler(templateHandlerNew),
                new IsNotPrimaryColumnAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsNotPrimaryColumnALastAutoColumnContextHandler(templateHandlerNew),
                new IsNotPrimaryColumnNotAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsNotPrimaryColumnNotALastAutoColumnContextHandler(templateHandlerNew),
                new IsPrimaryColumnAFirstPrimaryColumnContextHandler(templateHandlerNew),
                new IsPrimaryColumnALastPrimaryColumnContextHandler(templateHandlerNew),
                new IsPrimaryColumnNotAFirstAutoColumnContextHandler(templateHandlerNew),
                new IsPrimaryColumnNotALastAutoColumnContextHandler(templateHandlerNew),
                new IsIndexedColumnAFirstIndexedColumnContextHandler(templateHandlerNew),
                new NotAutoColumnIndexColumnContextHandler(templateHandlerNew),
                new NotNullColumnIndexColumnContextHandler(templateHandlerNew),
                new NotPrimaryColumnIndexColumnContextHandler(templateHandlerNew),
                new PrimaryColumnIndexColumnContextHandler(templateHandlerNew),
                //Table
                new ForEachAutoGeneratedValueColumnTableContextHandler(templateHandlerNew),
                new ForEachColumnTableContextHandler(templateHandlerNew),
                new ForEachIndexedColumnTableContextHandler(templateHandlerNew),
                new ForEachNotIndexedColumnTableContextHandler(templateHandlerNew),
                new ForEachNotAutoGeneratedValueColumnTableContextHandler(templateHandlerNew),
                new ForEachNotNullColumnTableContextHandler(templateHandlerNew),
                new ForEachNotPrimaryKeyColumnTableContextHandler(templateHandlerNew),
                new ForEachPrimaryKeyColumnTableContextHandler(templateHandlerNew),
                new TableNameTableContextHandler(templateHandlerNew),
                new TableSchemaTableContextHandler(templateHandlerNew),
                new WhenHasAutoGeneratedValueColumnTableContextHandler(templateHandlerNew),
                new WhenHasIndexColumnTableContextHandler(templateHandlerNew),
                new WhenHasNotIndexColumnTableContextHandler(templateHandlerNew),
                //Database
                new DatabaseNameDatabaseContextHandler(templateHandlerNew),
                new ConnectionStringDatabaseContextHandler(templateHandlerNew),
                new ForEachTableDatabaseContextHandler(templateHandlerNew),
                //Functions
                new FirstLetterToUpperCaseFunctionTemplateHandler(templateHandlerNew),
                new ReplaceWithFunctionTemplateHandler(templateHandlerNew),
        };
        }


        public IList<T> GetHanlders<T>() where T : ITemplateContextHandler
        {
            return _contextHandlers
                .Where(m => m is T && m != null)
                .Select(m => (T)m)
                .ToList();
        }

    }
}
