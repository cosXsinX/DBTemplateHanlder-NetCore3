using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public interface IConstraintVisitorCopier
    {
        public IConstraintVisitorContext CopyWithLevelOverride(IConstraintVisitorContext copied,int depthLevel)
        {
            IConstraintVisitorContext result = new ConstraintVisitorContext();
            result.Level = depthLevel;
            result.VisitStartPoint = copied.VisitStartPoint;
            return result;
        }
    }
}
