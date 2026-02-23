using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Subscription
    {

        public Guid SubscriptionId { get; set; }

        // Plan info
        public string SubscriptionName { get; set; } = null!;          // e.g. "Individual"
        public string? Description { get; set; }

        // Pricing
        public decimal SubscriptionPrice { get; set; }                  // 143200

        // Billing
        public string BillingPeriod { get; set; } = "Month"; // Month / Year

        // Status
        public bool IsActive { get; set; } = true;

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // navigation convenience
        public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } = new List<SubscriptionPayment>();

    }
}
