using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.inputs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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


        [HttpPost("register")]
        public IActionResult RegisterUser(UserInput userInput)
        {

            using (var context = new WoWDbContext(_configuration))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userInput.userPassword);

                User user = new User();
                user.userName = userInput.userName;
                user.userEmail = userInput.userEmail;
                user.userPassword = passwordHash;

                context.Users.Add(user);
                try {
                    context.SaveChanges();
                }catch (Exception e) {
                    return StatusCode(500, "Etwas ist schief gelaufen: "+e.InnerException);
                }
                

            }

            return Ok("User wurde erfolgreich erstellt");
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginInput loginInput)
        {

            using (var context = new WoWDbContext(_configuration))
            {
                User? user = context.Users.Where(e => e.userName == loginInput.userName).FirstOrDefault();
                
                if(user == null || !BCrypt.Net.BCrypt.Verify(loginInput.userPassword,user.userPassword))
                {
                    return BadRequest("Username oder Passwort sind falsch");
                }
                string token = CreateJWT(user);

                return Ok(token);

            }
        }


        private string CreateJWT(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
