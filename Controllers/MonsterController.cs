using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.responses;

namespace pwc_project.Controllers
{
    // Ein Controller für Monster in einer Webanwendung.
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MonstersController : Controller
    {
        private readonly ILogger<MonstersController> _logger;
        private readonly IConfiguration _configuration;

        // Konstruktor für den MonstersController.
        public MonstersController(ILogger<MonstersController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Endpunkt zum Abrufen aller Monster und ihrer Beute.
        [HttpGet]
        public IActionResult GetMonsters()
        {
            List<MonsterRespone> monsters = new List<MonsterRespone>();

            using (var context = new WoWDbContext(_configuration))
            {
                // Alle Monster aus der Datenbank abrufen.
                List<Monster> tempMonsters = context.Monsters.ToList();

                // Alle Beutedrops für Monster aus der Datenbank abrufen und mit den entsprechenden Ausrüstungsgegenständen verknüpfen.
                List<MonsterDropResponse> drops = context.MonsterDrop
                    .Join(context.Equipments,
                    p => p.equipmentID,
                    e => e.equipmentID,
                    (p, e) => new MonsterDropResponse
                    {
                        monsterID = p.monsterID,
                        equipmentID = p.equipmentID,
                        equipmentName = e.equipmentName,
                        dropChance = p.dropChance
                    }
                    ).ToList();

                // Für jedes Monster eine MonsterResponse erstellen und Beutedrops hinzufügen.
                foreach (var monster in tempMonsters)
                {
                    MonsterRespone tempResponse = new MonsterRespone();

                    tempResponse.monster = monster;
                    tempResponse.monsterDrops = drops.FindAll(e => e.monsterID == monster.monsterID);

                    monsters.Add(tempResponse);
                }
            }

            // OK-Status und die Liste der Monster mit Beutedrops zurückgeben.
            return Ok(monsters);
        }

        // Endpunkt zum Abrufen eines einzelnen Monsters anhand seiner ID.
        [HttpGet("{monsterID}")]
        public IActionResult GetMonster(int monsterID)
        {
            MonsterRespone monster = new MonsterRespone();

            using (var context = new WoWDbContext(_configuration))
            {
                // Das Monster mit der angegebenen ID aus der Datenbank abrufen.
                Monster? tempMonster = context.Monsters.Where(e => e.monsterID == monsterID).FirstOrDefault();

                // Wenn das Monster nicht gefunden wurde, eine entsprechende Fehlermeldung zurückgeben.
                if (tempMonster == null)
                {
                    return NotFound($"Monster mit der ID {monsterID} gibt es nicht!");
                }

                // Alle Beutedrops für das Monster aus der Datenbank abrufen und mit den entsprechenden Ausrüstungsgegenständen verknüpfen.
                List<MonsterDropResponse> drops = context.MonsterDrop
                    .Where(e => e.monsterID == monsterID)
                    .Join(context.Equipments,
                    p => p.equipmentID,
                    e => e.equipmentID,
                    (p, e) => new MonsterDropResponse
                    {
                        monsterID = p.monsterID,
                        equipmentID = p.equipmentID,
                        equipmentName = e.equipmentName,
                        dropChance = p.dropChance
                    }
                    ).ToList();

                // Das Monster und seine Beutedrops in der MonsterResponse speichern.
                monster.monster = tempMonster;
                monster.monsterDrops = drops.FindAll(e => e.monsterID == tempMonster.monsterID);
            }

            // OK-Status und das Monster mit seinen Beutedrops zurückgeben.
            return Ok(monster);
        }
    }
}
