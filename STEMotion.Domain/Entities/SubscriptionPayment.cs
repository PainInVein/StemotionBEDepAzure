using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class SubscriptionPayment
    {
        public Guid SubscriptionPaymentId { get; set; }

        // FK
        public Guid PaymentId { get; set; }
        public Guid SubscriptionId { get; set; }

        // PayOS / Bank result
        public string? Code { get; set; }             
        public string? Description { get; set; }     // "success"
        public bool? Success { get; set; }

        // Transaction info
        public string? AccountNumber { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }                 

        public long? OrderCode { get; set; }                     
        public string? Reference { get; set; }                 
        public string? PaymentLinkId { get; set; }

        public DateTime? TransactionDateTime { get; set; }

        // Counter account (bank)
        public string? CounterAccountBankId { get; set; }
        public string? CounterAccountName { get; set; }
        public string? CounterAccountNumber { get; set; }

        // Navigation
        public virtual Payment Payment { get; set; } = null!;
        public virtual Subscription Subscription { get; set; } = null!;
    }
}
