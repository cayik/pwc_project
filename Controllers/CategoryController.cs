using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;

namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class EquipmentSlotController : Controller
    {
        private readonly ILogger<EquipmentSlotController> _logger;
        private readonly IConfiguration _configuration;

        public EquipmentSlotController(ILogger<EquipmentSlotController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
