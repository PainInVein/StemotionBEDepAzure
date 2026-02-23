using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs
{
    public class PaymentInfoRequestDTO
    {
        public Guid UserId { get; set; }

        public SubscriptionInfo SubscriptionInfo { get; set; } = new SubscriptionInfo();
    }

    public class SubscriptionInfo
    {
        public Guid SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }     
        
        public long SubscriptionPrice { get; set; }              

        public string BillingPeriod { get; set; } 
    }
}
