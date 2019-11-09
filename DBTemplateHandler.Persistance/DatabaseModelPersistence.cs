using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Persistance
{
    public class DatabaseModelPersistence
    {
        public string Name { get; set; }
        public IDatabaseModel DatabaseModel { get; set; }
    }
}
