using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models
{
    public class SQLServerSchemasModel
    {
        public string name { get; set; }
        public int schema_id { get; set; }
        public int? principal_id { get; set; }
    }
}
