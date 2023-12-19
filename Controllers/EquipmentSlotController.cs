using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;


namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class MonsterDropController : Controller
    {
        private readonly ILogger<MonsterDropController> _logger;
        private readonly IConfiguration _configuration;

        public MonsterDropController(ILogger<MonsterDropController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
