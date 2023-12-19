using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;


namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
