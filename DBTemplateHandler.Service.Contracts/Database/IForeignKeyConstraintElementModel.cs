using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Service.Contracts.Database
{
    public interface IForeignKeyConstraintElementModel
    {
        public IColumnReferenceModel Primary { get; set; }
        public IColumnReferenceModel Foreign { get; set; }
    }
}
