using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.DO
{
    public class DO_TokenGeneration
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
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? TokenType { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
    public class DO_OTP
    {
        public int Id { get; set; }
        public string Otptype { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public decimal Otp { get; set; }
        public DateTime GeneratedOn { get; set; }
        public DateTime? ConfirmedOn { get; set; }

    }
}
