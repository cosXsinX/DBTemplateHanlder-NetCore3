using DBTemplateHander.DatabaseModel.Import.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBTemplateHander.DatabaseModel.Import
{
    public class ImporterProxy
    {
        private readonly IDictionary<Regex, IImporter> ImporterByConnectionStringRegex
            = new Dictionary<Regex, IImporter>()
            {
                {new Regex($"Data Source=ServerName;Initial Catalog=DatabaseName; User ID = UserName; Password=Password"),
                    new SQLServerImporter() }
            };

        public IList<IImporter> Resolve(string connectionString)
        {
            var regexes = ImporterByConnectionStringRegex.Keys;
            var matchingRegexes = regexes.Where(m => m.IsMatch(connectionString)).ToList();
            return regexes.Select(regex => ImporterByConnectionStringRegex[regex]).ToList();
        }
    }
}
