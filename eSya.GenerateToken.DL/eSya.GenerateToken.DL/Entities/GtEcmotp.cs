using System;
using System.Collections.Generic;

namespace eSya.GenerateToken.DL.Entities
{
    public partial class GtEcmotp
    {
        public int Id { get; set; }
        public string Otptype { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public decimal Otp { get; set; }
        public DateTime GeneratedOn { get; set; }
        public DateTime? ConfirmedOn { get; set; }
    }
}
