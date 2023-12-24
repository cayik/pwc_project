using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using pwc_project.Models;
using pwc_project.Models.database;
using pwc_project.Models.inputs;
using pwc_project.Models.responses;

namespace pwc_project.Controllers
{

    [ApiController]
    [Authorize]
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
        public IActionResult GetCharater(int characterID)
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


        [HttpPost("{characterID}/equip")]
        public IActionResult EquipItem(int characterID, List<EquipInput> equipInputs)
        {

            using (var context = new WoWDbContext(_configuration))
            {
                Character? character = context.Characters.Where(e => e.characterID == characterID).FirstOrDefault();
                List<Equipment> equipments = context.Equipments.ToList();
                List<Equipment> mentionedEquipments = new List<Equipment>();
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


                //Existiert der Charakter?
                if (character == null)
                {
                    return NotFound($"Charakter mit der ID {characterID} wurde nicht gefunden");
                }
                //Existiert der Gegenstand?
                foreach (var equipInput in equipInputs) {
                    Equipment? tempEquipment = equipments.Find(e => e.equipmentID == equipInput.equipmentID);
                    if (tempEquipment == null)
                    {
                        return NotFound($"Equipment mit der ID {equipInput.equipmentID} nicht gefunden!");
                    }
                    else
                    {
                        mentionedEquipments.Add(tempEquipment);
                    }
                }
                //Check ob in der List mehere gleiche Kategorieren hinzugefügt werden
                foreach(var mentionedEquipment in mentionedEquipments)
                {
                    if(mentionedEquipments.FindAll(e => e.categoryID == mentionedEquipment.categoryID).Count() > 1) {

                        return BadRequest("Es können nicht mehere Gegenstände mit der selben Kategorie ausgerüstet werden");
                    }
                }

                List<EquipInputDetailed> equipInputDetails = equipInputs
                    .Join(mentionedEquipments,
                    e => e.equipmentID, p => p.equipmentID,
                    (e, p) => new EquipInputDetailed
                    {
                        equipmentID = p.equipmentID,
                        replace = e.replace,
                        categoryID = p.categoryID
                    }).ToList();

                //Ist dem Charakter bereits ein Gegenstand aus der Kategorie zugewiesen?
                foreach (var equipInputDetail in equipInputDetails)
                {
                    // Hat bereits einen Slots ausgerüstet 
                    EquipmentSlotResponse? tempSlot = slots.Find(e => e.categoryID == equipInputDetail.categoryID);
                    if (tempSlot != null)
                    {
                        //ist es auf Override gestellt
                        if (equipInputDetail.replace)
                        {
                            context.EquipmentSlot.Remove(context.EquipmentSlot.Single(e => e.equipmentID == tempSlot.equipmentID && e.characterID == tempSlot.characterID));
                            EquipmentSlot slot = new EquipmentSlot();
                            slot.characterID = characterID;
                            slot.equipmentID = equipInputDetail.equipmentID;
                            context.EquipmentSlot.Add(slot);
                        }
                        else
                        {
                            return StatusCode(500, "Der Slot wird bereits von einem anderen Gegenstand benutzt");
                        }
                    }
                    else
                    {
                        EquipmentSlot slot = new EquipmentSlot();
                        slot.characterID = characterID;
                        slot.equipmentID = equipInputDetail.equipmentID;
                        context.EquipmentSlot.Add(slot);
                    }
                }
                context.SaveChanges();
            }
            return Ok();
        }
    }
}
