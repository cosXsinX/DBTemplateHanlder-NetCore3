using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class ProcessorDatabaseContext : IDatabaseContext
    {
        public IDatabaseModel Database { get; set; }
        public ITableModel Table { get; set; }
        public IColumnModel Column { get; set; }
        public IForeignKeyConstraintModel ForeignKeyConstraint { get; set; }
    }
}
