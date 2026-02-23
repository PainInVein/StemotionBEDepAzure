using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class SubscriptionResponseDTO
    {
        public Guid SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }        
        public string? Description { get; set; }

        public decimal SubscriptionPrice { get; set; }               

        public string BillingPeriod { get; set; } 
    }
}
