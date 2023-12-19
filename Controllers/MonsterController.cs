using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;

namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class MonsterController : Controller
    {
        private readonly ILogger<MonsterController> _logger;
        private readonly IConfiguration _configuration;

        public MonsterController(ILogger<MonsterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetMonsters()
        {
            List<Monster> monsters = new List<Monster>();

            using (var context = new WoWDbContext(_configuration))
            {
                monsters = context.Monsters.ToList();
            }

            return Ok(monsters);
        }
    }
}
