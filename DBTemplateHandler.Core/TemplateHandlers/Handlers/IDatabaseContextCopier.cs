using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public interface IDatabaseContextCopier
    {
        IDatabaseContext Copy(IDatabaseContext copied);
        IDatabaseContext CopyWithOverride(IDatabaseContext copied, IColumnModel column);
        IDatabaseContext CopyWithOverride(IDatabaseContext copied, IDatabaseModel database);
        IDatabaseContext CopyWithOverride(IDatabaseContext copied, IForeignKeyConstraintModel constraint);
        IDatabaseContext CopyWithOverride(IDatabaseContext copied, ITableModel table);
    }
}