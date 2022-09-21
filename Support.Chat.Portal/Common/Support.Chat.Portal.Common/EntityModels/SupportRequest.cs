using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.Chat.Portal.Common.Models
{
    public class SupportRequest
    {
        public string User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public bool Resolved { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
