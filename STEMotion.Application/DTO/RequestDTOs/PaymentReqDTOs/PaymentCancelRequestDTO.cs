using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs
{
    public class PaymentCancelRequestDTO
    {
        public string? Code { get; set; }           // "00"

        public string? PaymentLinkId { get; set; }  // PayOS Payment Link ID

        public string? Status { get; set; }         // "CANCELLED", "PAID", etc.

        public long OrderCode { get; set; }        // Your internal order code – most important!

        public string? Cancel { get; set; }         // "true" / "false"
    }
}
