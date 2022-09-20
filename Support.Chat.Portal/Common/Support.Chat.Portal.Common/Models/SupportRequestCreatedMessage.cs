﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support.Chat.Portal.Common.Models
{
    public class SupportRequestCreatedMessage : ISupportRequestCreatedMessage
    {
        public Guid MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public SupportRequest SupportRequest { get; set; }
    }

    public interface ISupportRequestCreatedMessage
    {
        public Guid MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public SupportRequest SupportRequest { get; set; }
    }
}
