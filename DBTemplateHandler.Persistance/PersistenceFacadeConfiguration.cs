using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Persistance
{
    public class PersistenceFacadeConfiguration
    {
        public string TemplatesFolderPath { get; set; }
        public string DatabaseModelsFolderPath { get; set; }
        public string TypeSetFolderPath { get; set; }
        public string TypeMappingFolderPath { get; set; }
    }
}
