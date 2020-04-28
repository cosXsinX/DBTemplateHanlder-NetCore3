using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Service.Contracts.Database
{
    public interface IForeignKeyConstraintModel
    {
        public string ConstraintName { get; set; }
        public IList<IForeignKeyConstraintElementModel> Elements { get; set; }
    }
}
