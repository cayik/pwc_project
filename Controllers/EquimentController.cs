using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.responses;

namespace pwc_project.Controllers
{
    // Ein Controller für Ausrüstungsgegenstände in einer Webanwendung.
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EquipmentController : Controller
    {
        private readonly ILogger<EquipmentController> _logger;
        private readonly IConfiguration _configuration;

        // Konstruktor für den EquipmentController.
        public EquipmentController(ILogger<EquipmentController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Endpunkt zum Abrufen von Ausrüstungsgegenständen basierend auf Kategorie und/oder Name.
        [HttpGet]
        public IActionResult GetEquipment(String? category, String? name)
        {
            // Listen für Ausrüstungsgegenstände und Ausrüstungsstatistiken initialisieren.
            List<EquipmentResponse> equipments = new List<EquipmentResponse>();
            List<EquipmentStat> stats = new List<EquipmentStat>();

            using (var context = new WoWDbContext(_configuration))
            {
                // Alle Ausrüstungsgegenstände aus der Datenbank abrufen und ihre Kategorien zusammenführen.
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

                // Alle Ausrüstungsstatistiken abrufen.
                stats = context.EquipmentStat.ToList();

                // Wenn eine Kategorie angegeben ist, die Liste der Ausrüstungsgegenstände filtern.
                if (category != null)
                {
                    equipments = equipments.FindAll(e => e.categoryName.ToLower().Contains(category.ToLower()));
                }

                // Wenn ein Name angegeben ist, die Liste der Ausrüstungsgegenstände filtern.
                if (name != null)
                {
                    equipments = equipments.FindAll(e => e.equipmentName.ToLower().Contains(name.ToLower()));
                }

                // Die Ausrüstungsstatistiken für jeden Ausrüstungsgegenstand hinzufügen.
                foreach (var equipment in equipments)
                {
                    equipment.equipmentStat = stats.FindAll(e => e.equipmentID == equipment.equipmentID);
                }
            }

            // Wenn keine Ausrüstungsgegenstände gefunden wurden, eine entsprechende Fehlermeldung zurückgeben.
            if (equipments.Count() == 0)
            {
                return NotFound($"Equipment nicht gefunden!");
            }
            else
            {
                // OK-Status und die Liste der Ausrüstungsgegenstände zurückgeben.
                return Ok(equipments);
            }
        }

        // Endpunkt zum Abrufen eines einzelnen Ausrüstungsgegenstands anhand seiner ID.
        [HttpGet("{equipmentID}")]
        [Authorize]
        public IActionResult GetEquipment(int equipmentID)
        {
            EquipmentResponse? equipment = new EquipmentResponse();
            List<EquipmentStat> stats = new List<EquipmentStat>();

            using (var context = new WoWDbContext(_configuration))
            {
                // Den Ausrüstungsgegenstand mit der angegebenen ID aus der Datenbank abrufen und seine Kategorie zusammenführen.
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

                // Wenn der Ausrüstungsgegenstand nicht gefunden wurde, eine entsprechende Fehlermeldung zurückgeben.
                if (equipment == null)
                {
                    return NotFound($"Equipment mit der ID {equipmentID} nicht gefunden!");
                }

                // Alle Ausrüstungsstatistiken abrufen und dem Ausrüstungsgegenstand hinzufügen.
                stats = context.EquipmentStat.ToList();
                equipment.equipmentStat = stats.FindAll(e => e.equipmentID == equipment.equipmentID);
            }

            // OK-Status und den Ausrüstungsgegenstand zurückgeben.
            return Ok(equipment);
        }
    }
}
