using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtTokm01
    {
        public string TokenType { get; set; } = null!;
        public string TokenPrefix { get; set; } = null!;
        public string TokenDesc { get; set; } = null!;
        public int TokenNumberLength { get; set; }
        public string ConfirmationUrl { get; set; } = null!;
        public string? QrcodeUrl { get; set; }
        public int DisplaySequence { get; set; }
        public bool IsEnCounter { get; set; }
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
