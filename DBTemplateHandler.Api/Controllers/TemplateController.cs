using DBTemplateHandler.Core.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTemplateHandler.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TemplateController : ControllerBase
    {
        [HttpPost]
        public string ProcessTemplate(DatabaseModel databaseDescriptionPOJO)
        {
            return null;
        }

    }
}
