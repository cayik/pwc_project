using Microsoft.AspNetCore.Mvc;
using pwc_project.Models;


namespace pwc_project.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IConfiguration _configuration;

        public CategoryController(ILogger<CategoryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        
    }
}
