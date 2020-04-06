using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Service.Contracts.Database
{
    public interface IForeignKeyConstraintModel
    {
        public IList<IColumnModel> PrimaryKeyColumns { get; set; }
        public IList<IColumnModel> ForeignKeyColumns { get; set; }
    }
}
