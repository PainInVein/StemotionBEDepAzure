using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class PaymentResponseDTO
    {
        public string CheckoutUrl { get; set; }
        public string PaymentLinkId { get; set; }
        public long OrderCode { get; set; }
    }
}
