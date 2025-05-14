using Microsoft.AspNetCore.Mvc;
using PcController.Interfaces;

namespace PcController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HAController(IPluginManager manager) : ControllerBase
    {
        
    }
}
