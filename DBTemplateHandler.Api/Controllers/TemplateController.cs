using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace DBTemplateHandler.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TemplateController : ControllerBase
    {
        private readonly InputModelHandler inputModelHandler = new InputModelHandler();

        [HttpPost]
        public IActionResult Process(IDatabaseTemplateHandlerInputModel input)
        {
            if (input == null) return BadRequest(new { Error = "Submitted input is null" });
            if (input.DatabaseModel == null) return BadRequest(new { Error = "Submitted database is null" });
            var results = inputModelHandler.Process(input);
            return Ok(results);
        }
    }
}
