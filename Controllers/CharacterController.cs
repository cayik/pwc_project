using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.responses;

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

        [HttpGet]
        public IActionResult GetCharaters(string? characterName)
        {

            List<Character> characters = new List<Character>();

            using (var context = new WoWDbContext(_configuration))
            {
                characters = context.Characters.ToList();

                if(characterName != null)
                {
                    characters = characters.FindAll(e => e.characterName.ToLower().Contains(characterName.ToLower()));
                }

            }

            return Ok(characters);

        }

        [HttpGet("{characterID}")]
        public IActionResult GetCharaters(int characterID)
        {

            CharacterResponse character = new CharacterResponse();

            using (var context = new WoWDbContext(_configuration))
            {
                Character? tempChar = context.Characters.Where(e => e.characterID == characterID).FirstOrDefault();

                if (tempChar == null)
                {
                    return NotFound($"Charakter mit der ID {characterID} wurde nicht gefunden");
                }

                List<EquipmentSlotResponse> slots = context.EquipmentSlot.Where(e => e.characterID == characterID)
                            .Join(context.Equipments, p => p.equipmentID, e => e.equipmentID,
                            (p, e) => new {
                                p,
                                e
                            }
                            ).Join(context.Categories, c => c.e.categoryID, ce => ce.categoryID,
                            (c, ce) => new EquipmentSlotResponse
                            {
                                equipmentID = c.p.equipmentID,
                                characterID = c.p.characterID,
                                equipmentName = c.e.equipmentName,
                                categoryID = c.e.categoryID,
                                categoryName = ce.categoryName
                            }
                            ).ToList();

                character.characterObject = tempChar;

                character.equipmentSlots = slots;

            }

            return Ok(character);

        }

    }
}
