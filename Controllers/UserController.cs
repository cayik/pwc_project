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
    // Ein Controller für Benutzer in einer Webanwendung.
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        // Konstruktor für den UserController.
        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Endpunkt zum Registrieren eines neuen Benutzers.
        [HttpPost("register")]
        public IActionResult RegisterUser(UserInput userInput)
        {
            using (var context = new WoWDbContext(_configuration))
            {
                // Das Passwort des Benutzers hashen.
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userInput.userPassword);

                // Einen neuen Benutzer erstellen und in die Datenbank einfügen.
                User user = new User();
                user.userName = userInput.userName;
                user.userEmail = userInput.userEmail;
                user.userPassword = passwordHash;

                context.Users.Add(user);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return StatusCode(500, "Etwas ist schief gelaufen: " + e.InnerException);
                }
            }

            // Erfolgreiche Nachricht zurückgeben.
            return Ok("User wurde erfolgreich erstellt");
        }

        // Endpunkt zum Einloggen eines Benutzers.
        [HttpPost("login")]
        public IActionResult LoginUser(LoginInput loginInput)
        {
            using (var context = new WoWDbContext(_configuration))
            {
                // Den Benutzer anhand des Benutzernamens aus der Datenbank abrufen.
                User? user = context.Users.Where(e => e.userName == loginInput.userName).FirstOrDefault();

                // Wenn der Benutzer nicht gefunden wurde oder das Passwort nicht übereinstimmt, eine Fehlermeldung zurückgeben.
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginInput.userPassword, user.userPassword))
                {
                    return BadRequest("Username oder Passwort sind falsch");
                }

                // JWT-Token erstellen und zurückgeben.
                string token = CreateJWT(user);

                return Ok(token);
            }
        }

        // Methode zum Erstellen eines JWT-Tokens für den Benutzer.
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
