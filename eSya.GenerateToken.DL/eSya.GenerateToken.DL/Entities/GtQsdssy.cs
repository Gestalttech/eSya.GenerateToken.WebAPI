using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtQsdssy
    {
        public int BusinessKey { get; set; }
        public int DisplayId { get; set; }
        public string DisplayIpaddress { get; set; } = null!;
        public string DisplayScreenType { get; set; } = null!;
        public string DisplayUrl { get; set; } = null!;
        public string QueryString { get; set; } = null!;
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
