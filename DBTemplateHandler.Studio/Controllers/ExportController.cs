using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DBTemplateHandler.Studio.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBTemplateHandler.Studio.Controllers
{
    public class ExportController : Controller
    {
        private readonly ExportService exportService;
        private readonly DBTemplateService dBTemplateService;

        public ExportController(DBTemplateService dBTemplateService, ExportService exportService)
        {
            if (exportService == null) throw new ArgumentNullException(nameof(exportService));
            if (dBTemplateService == null) throw new ArgumentNullException(nameof(dBTemplateService));
            this.exportService = exportService;
            this.dBTemplateService = dBTemplateService;
        }


        Regex exportIdFilterRegex = new Regex("^\\((.*)\\)->\\((.*)\\)\\.[zZ][iI][pP]$");

        // GET: /<controller>/
        [HttpGet()]
        public IActionResult Export(string id)
        {
            if(!exportIdFilterRegex.IsMatch(id)) return NoContent();
            var splittedId = id.Split(")->(");
            var databaseModelName = splittedId[0].Substring(1, splittedId[0].Length - 1);
            var templateGroupName = splittedId[1].Substring(0, splittedId[1].Length - 5);
            if (templateGroupName == null) return NoContent();
            if (databaseModelName == null) return NoContent();
            var decodedDatabaseModelName = databaseModelName;
            var decodedtemplateGroupName = templateGroupName;
            var databaseModel = dBTemplateService.GetDatabaseModelByPersistenceNameSync(decodedDatabaseModelName);
            var templateModelGroup = dBTemplateService.GetTemplateModelByPersistenceNameSync(decodedtemplateGroupName);
            var resultFiles = dBTemplateService.Process(templateModelGroup, databaseModel).ToList();
            var zipArchiveName = $"{decodedDatabaseModelName}-{decodedtemplateGroupName}";
            var zipArchiveFilePath = exportService.ToZipArchive(zipArchiveName, resultFiles);
            FileStream fs = System.IO.File.Open(zipArchiveFilePath, FileMode.Open);
            return new FileStreamResult(fs, "application/zip");
        }
    }
}
