using DBTemplateHandler.Service.Contracts.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database
{
    public class ColumnReferenceModel : IColumnReferenceModel
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
    }
}
