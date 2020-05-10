using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public interface IDatabaseContext
    {
        IColumnModel Column { get; set; }
        IDatabaseModel Database { get; set; }
        IForeignKeyConstraintModel ForeignKeyConstraint { get; set; }
        IConstraintVisitorContext ConstraintVisitorContext { get; set; }
        ITableModel Table { get; set; }
    }
}