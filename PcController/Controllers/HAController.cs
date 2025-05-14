using Microsoft.AspNetCore.Mvc;
using PcController.Interfaces;

namespace PcController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HAController(IPluginManager manager) : ControllerBase
    {
        [HttpGet("~/get-plugins")]
        public IActionResult GetPlugins()
        {
            return Ok(manager.GetAllPlugins().Select(x => new
            {
                x.Name,
                x.Description
            }));
        }

        [HttpPost("~/excecute")]
        public async Task<IActionResult> Excecute([FromQuery] string name)
        {
            var plugin = manager.FindPlugin(name);
            if (plugin == null)
                return NotFound();

            var result = await plugin.Execute();

            if (result.IsSuccess)
                return Ok();
            else
                return StatusCode(500, result.Error.Text);
        }
    }
}