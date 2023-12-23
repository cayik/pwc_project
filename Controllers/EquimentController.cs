using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.responses;

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
        public IActionResult GetEquipment(String? category, String? name) 
        {
            List<EquipmentResponse> equipments = new List<EquipmentResponse>();
            List<EquipmentStat> stats = new List<EquipmentStat>();   
            
            using(var context = new WoWDbContext(_configuration))
            {
                equipments = context.Equipments
                    .Join(context.Categories,
                    p => p.categoryID,
                    e => e.categoryID,
                    (p, e) => new EquipmentResponse
                    {
                        equipmentID = p.equipmentID,
                        equipmentName = p.equipmentName,
                        categoryID = e.categoryID,
                        categoryName = e.categoryName
                    }
                    ).ToList();

                stats = context.EquipmentStat.ToList();

                if(category != null)
                {
                    equipments = equipments.FindAll(e => e.categoryName.ToLower().Contains(category.ToLower()));
                }

                if(name != null)
                {
                    equipments = equipments.FindAll(e => e.equipmentName.ToLower().Contains(name.ToLower()));
                }

                foreach(var equipment in equipments)
                {
                    equipment.equipmentStat = stats.FindAll(e => e.equipmentID == equipment.equipmentID);
                }

            }

            if(equipments.Count() == 0)
            {
                return NotFound($"Equipment nicht gefunden!");
            }
            else
            {
                return Ok(equipments);
            }
        }

        [HttpGet("{equipmentID}")]
        public IActionResult GetEquipment(int equipmentID)
        {
            EquipmentResponse? equipment = new EquipmentResponse();
            List<EquipmentStat> stats = new List<EquipmentStat>();

            using (var context = new WoWDbContext(_configuration))
            {
                equipment = context.Equipments
                    .Join(context.Categories,
                    p => p.categoryID,
                    e => e.categoryID,
                    (p, e) => new EquipmentResponse
                    {
                        equipmentID = p.equipmentID,
                        equipmentName = p.equipmentName,
                        categoryID = e.categoryID,
                        categoryName = e.categoryName
                    }
                    ).FirstOrDefault();


                if (equipment == null)
                {
                    return NotFound($"Equipment nicht mit der ID {equipmentID} nicht gefunden!");
                }

                stats = context.EquipmentStat.ToList();

                equipment.equipmentStat = stats.FindAll(e => e.equipmentID == equipment.equipmentID);

            }
            return Ok(equipment);
        }


    }
}
