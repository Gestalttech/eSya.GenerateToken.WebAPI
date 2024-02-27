using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtTokm03
    {
        public int BusinessKey { get; set; }
        public string TokenPrefix { get; set; } = null!;
        public int FloorId { get; set; }
        public string CounterNumber { get; set; } = null!;
        public string CounterKey { get; set; } = null!;
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
