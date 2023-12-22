using pwc_project.Models.database;

namespace pwc_project.Models.responses
{
    public class MonsterRespone
    {
        public Monster monster {  get; set; }

        public List<MonsterDropResponse> monsterDrops { get; set; }
    }
}
