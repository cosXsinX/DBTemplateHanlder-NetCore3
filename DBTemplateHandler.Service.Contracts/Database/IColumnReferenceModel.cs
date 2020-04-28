using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Service.Contracts.Database
{
    public interface IColumnReferenceModel
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
    }
}
