using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;

namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class EquipmentController : Controller
    {
        private readonly ILogger<EquipmentController> _logger;
        private readonly IConfiguration _configuration;

        public EquipmentController(ILogger<EquipmentController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetEquipment(String category, String name) 
        {
            List<Equipment> equipment = new List<Equipment>();

            
            using(var context = new WoWDbContext(_configuration))
            {
                List<Equipment> tempList = context.Equipments.ToList();


            }
        }

        
    }
}
