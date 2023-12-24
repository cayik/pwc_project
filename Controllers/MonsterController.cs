using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.responses;

namespace pwc_project.Controllers
{

    [ApiController]
    [Authorize]
    [Route("[controller]")]

    public class MonstersController : Controller
    {
        private readonly ILogger<MonstersController> _logger;
        private readonly IConfiguration _configuration;

        public MonstersController(ILogger<MonstersController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetMonsters()
        {
            List<MonsterRespone> monsters = new List<MonsterRespone>();

            using (var context = new WoWDbContext(_configuration))
            {
                List<Monster> tempMonsters = context.Monsters.ToList();
                List<MonsterDropResponse> drops = context.MonsterDrop
                    .Join(context.Equipments,
                    p => p.equipmentID,
                    e => e.equipmentID,
                    (p,e) => new MonsterDropResponse{
                        monsterID = p.monsterID,
                        equipmentID = p.equipmentID,
                        equipmentName = e.equipmentName,
                        dropChance = p.dropChance
                    }
                    ).ToList();

                foreach (var monster in tempMonsters)
                {
                    MonsterRespone tempResponse = new MonsterRespone();

                    tempResponse.monster = monster;
                    tempResponse.monsterDrops = drops.FindAll(e => e.monsterID == monster.monsterID);

                    monsters.Add(tempResponse);
                }
            }

            return Ok(monsters);
        }


        [HttpGet("{monsterID}")]
        public IActionResult GetMonster(int monsterID)
        {
            MonsterRespone monster = new MonsterRespone();

            using (var context = new WoWDbContext(_configuration))
            {
                Monster? tempMonster = context.Monsters.Where(e => e.monsterID == monsterID).FirstOrDefault();

                if (tempMonster == null)
                {
                    return NotFound($"Monster mit der ID {monsterID} gibt es nicht!");
                }

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

                monster.monster = tempMonster;
                monster.monsterDrops = drops.FindAll(e => e.monsterID == tempMonster.monsterID);
            }

            return Ok(monster);
        }
    }
}
