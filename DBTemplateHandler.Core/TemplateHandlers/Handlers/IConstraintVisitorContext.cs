using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public interface IConstraintVisitorContext
    {
        int Level { get; set; }
        IForeignKeyConstraintModel VisitStartPoint { get; set; }
    }
}