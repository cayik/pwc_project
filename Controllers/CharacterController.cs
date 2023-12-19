using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;

namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class CharacterController : Controller
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly IConfiguration _configuration;

        public CharacterController(ILogger<CharacterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
