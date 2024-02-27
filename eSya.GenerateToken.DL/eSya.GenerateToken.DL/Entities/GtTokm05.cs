using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtTokm05
    {
        public int BusinessKey { get; set; }
        public DateTime TokenDate { get; set; }
        public string TokenKey { get; set; } = null!;
        public string TokenPrefix { get; set; } = null!;
        public int SequeueNumber { get; set; }
        public int? Isdcode { get; set; }
        public string? MobileNumber { get; set; }
        public bool TokenCalling { get; set; }
        public DateTime? TokenCallingTime { get; set; }
        public string? CallingCounter { get; set; }
        public string? CounterKey { get; set; }
        public int HoldOccurrence { get; set; }
        public int ReCallOccurrence { get; set; }
        public DateTime? ConfirmationTime { get; set; }
        public string TokenStatus { get; set; } = null!;
        public DateTime? CompletedTime { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
