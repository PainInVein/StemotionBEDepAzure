using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }

        public string Info { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Status { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public virtual SubscriptionPayment SubscriptionPayment { get; set; }

    }
}
