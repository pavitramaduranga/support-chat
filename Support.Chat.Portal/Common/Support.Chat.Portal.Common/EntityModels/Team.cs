using Support.Chat.Portal.Common.Enums;

namespace Support.Chat.Portal.Common.Models
{
    public class Team
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public Shift Shift { get; set; }
        public bool IsOverflow { get; set; }

        public ICollection<Agent> Agents { get; set; }
    }

}
