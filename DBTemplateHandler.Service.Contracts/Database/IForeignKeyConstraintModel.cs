using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface IForeignKeyConstraintModel
    {
        public string ConstraintName { get; set; }
        public IList<IForeignKeyConstraintElementModel> Elements { get; set; }
    }
}
