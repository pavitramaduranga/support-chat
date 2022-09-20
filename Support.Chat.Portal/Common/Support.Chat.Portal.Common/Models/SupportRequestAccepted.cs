using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.Chat.Portal.Common.Models
{
    public class SupportRequestAccepted
    {
        public Guid MessageId { get; set; }

        public bool Accepted { get; set; }
    }
}
