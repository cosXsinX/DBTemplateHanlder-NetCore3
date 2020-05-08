using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class DatabaseContextCopier : IDatabaseContextCopier
    {
        public IDatabaseContext Copy(IDatabaseContext copied)
        {
            return new ProcessorDatabaseContext()
            {
                Column = copied.Column,
                ForeignKeyConstraint = copied.ForeignKeyConstraint,
                Table = copied.Table,
                Database = copied.Database,
            };
        }

        public IDatabaseContext CopyWithOverride(IDatabaseContext copied, IColumnModel column)
        {
            return new ProcessorDatabaseContext()
            {
                Column = column,
                ForeignKeyConstraint = copied.ForeignKeyConstraint,
                Table = copied.Table,
                Database = copied.Database,
            };
        }

        public IDatabaseContext CopyWithOverride(IDatabaseContext copied, IForeignKeyConstraintModel constraint)
        {
            return new ProcessorDatabaseContext()
            {
                Column = copied.Column,
                ForeignKeyConstraint = constraint,
                Table = copied.Table,
                Database = copied.Database,
            };
        }

        public IDatabaseContext CopyWithOverride(IDatabaseContext copied, ITableModel table)
        {
            return new ProcessorDatabaseContext()
            {
                Table = table,
                Database = copied.Database,
            };
        }

        public IDatabaseContext CopyWithOverride(IDatabaseContext copied, IDatabaseModel database)
        {
            return new ProcessorDatabaseContext()
            {
                Database = database,
            };
        }
    }
}
