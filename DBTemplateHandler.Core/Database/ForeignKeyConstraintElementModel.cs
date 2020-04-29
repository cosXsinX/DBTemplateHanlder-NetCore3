using DBTemplateHandler.Service.Contracts.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database
{
    public class ForeignKeyConstraintElementModel : IForeignKeyConstraintElementModel
    {
        public IColumnReferenceModel Primary { get; set; }
        public IColumnReferenceModel Foreign { get; set; }
    }
}
