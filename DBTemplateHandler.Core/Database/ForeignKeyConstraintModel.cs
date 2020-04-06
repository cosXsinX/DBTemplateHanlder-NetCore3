using DBTemplateHandler.Service.Contracts.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database
{
    public class ForeignKeyConstraintModel : IForeignKeyConstraintModel
    {
        public IList<IColumnModel> PrimaryKeyColumns { get; set; }
        public IList<IColumnModel> ForeignKeyColumns { get; set; }
    }
}
