using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;


namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class EquipmentStatController : Controller
    {
        private readonly ILogger<EquipmentStatController> _logger;
        private readonly IConfiguration _configuration;

        public EquipmentStatController(ILogger<EquipmentStatController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
