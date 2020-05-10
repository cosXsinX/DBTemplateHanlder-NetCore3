using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class ConstraintVisitorContext : IConstraintVisitorContext
    {
        public int Level { get; set; }
        public IForeignKeyConstraintModel VisitStartPoint { get; set; }
    }
}
