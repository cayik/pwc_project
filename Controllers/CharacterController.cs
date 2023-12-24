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
    // Ein Controller für Charaktere in einer Webanwendung.
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CharacterController : Controller
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly IConfiguration _configuration;

        // Konstruktor für den CharacterController.
        public CharacterController(ILogger<CharacterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Endpunkt zum Abrufen von Charakteren.
        [HttpGet]
        public IActionResult GetCharaters(string? characterName)
        {
            // Eine Liste von Charakteren initialisieren.
            List<Character> characters = new List<Character>();

            using (var context = new WoWDbContext(_configuration))
            {
                // Alle Charaktere aus der Datenbank abrufen.
                characters = context.Characters.ToList();

                // Wenn ein Charaktername angegeben ist, filtere die Liste nach passenden Charakteren.
                if (characterName != null)
                {
                    characters = characters.FindAll(e => e.characterName.ToLower().Contains(characterName.ToLower()));
                }
            }

            // OK-Status und die Liste der Charaktere zurückgeben.
            return Ok(characters);
        }

        // Endpunkt zum Abrufen eines einzelnen Charakters anhand seiner ID.
        [HttpGet("{characterID}")]
        public IActionResult GetCharater(int characterID)
        {
            CharacterResponse character = new CharacterResponse();

            using (var context = new WoWDbContext(_configuration))
            {
                // Den Charakter mit der angegebenen ID aus der Datenbank abrufen.
                Character? tempChar = context.Characters.Where(e => e.characterID == characterID).FirstOrDefault();

                // Wenn der Charakter nicht gefunden wurde, eine entsprechende Fehlermeldung zurückgeben.
                if (tempChar == null)
                {
                    return NotFound($"Charakter mit der ID {characterID} wurde nicht gefunden");
                }

                // Ausrüstungsslots und Charakterinformationen abrufen und in der Response speichern.
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

            // OK-Status und den Charakter mit Ausrüstungsslots zurückgeben.
            return Ok(character);
        }

        // Endpunkt zum Ausrüsten von Gegenständen auf einem Charakter.
        [HttpPost("{characterID}/equip")]
        public IActionResult EquipItem(int characterID, List<EquipInput> equipInputs)
        {
            using (var context = new WoWDbContext(_configuration))
            {
                // Den Charakter mit der angegebenen ID aus der Datenbank abrufen.
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

                // Existiert der Charakter?
                if (character == null)
                {
                    return NotFound($"Charakter mit der ID {characterID} wurde nicht gefunden");
                }

                // Existiert der Gegenstand?
                foreach (var equipInput in equipInputs)
                {
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

                // Überprüfen, ob mehrere Gegenstände derselben Kategorie ausgerüstet werden.
                foreach (var mentionedEquipment in mentionedEquipments)
                {
                    if (mentionedEquipments.FindAll(e => e.categoryID == mentionedEquipment.categoryID).Count() > 1)
                    {
                        return BadRequest("Es können nicht mehrere Gegenstände mit derselben Kategorie ausgerüstet werden");
                    }
                }

                // Details der Ausrüstungseingabe erstellen.
                List<EquipInputDetailed> equipInputDetails = equipInputs
                    .Join(mentionedEquipments,
                    e => e.equipmentID, p => p.equipmentID,
                    (e, p) => new EquipInputDetailed
                    {
                        equipmentID = p.equipmentID,
                        replace = e.replace,
                        categoryID = p.categoryID
                    }).ToList();

                // Überprüfen, ob dem Charakter bereits ein Gegenstand derselben Kategorie zugewiesen ist.
                foreach (var equipInputDetail in equipInputDetails)
                {
                    EquipmentSlotResponse? tempSlot = slots.Find(e => e.categoryID == equipInputDetail.categoryID);
                    if (tempSlot != null)
                    {
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

                // Änderungen in der Datenbank speichern.
                context.SaveChanges();
            }

            // Erfolgreiche Antwort zurückgeben.
            return Ok("Erfolgreich ausgerüstet");
        }
    }
}
