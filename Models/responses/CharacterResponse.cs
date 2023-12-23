using pwc_project.Models.database;

namespace pwc_project.Models.responses
{
    public class CharacterResponse
    {
        public Character characterObject { get; set; }

        public List<EquipmentSlotResponse> equipmentSlots { get; set; }

    }
}
