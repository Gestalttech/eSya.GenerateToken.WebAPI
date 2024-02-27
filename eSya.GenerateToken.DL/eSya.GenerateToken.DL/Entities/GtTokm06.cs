using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtTokm06
    {
        public int BusinessKey { get; set; }
        public DateTime TokenDate { get; set; }
        public string TokenKey { get; set; } = null!;
        public int HoldOccurence { get; set; }
        public DateTime? HoldTime { get; set; }
        public DateTime? ReCallTime { get; set; }
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
