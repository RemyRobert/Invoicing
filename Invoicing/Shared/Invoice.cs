using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.Shared
{
    public class Invoice
    {
        public Invoice()
        {
        }

        public Invoice(string reference, string customer, decimal amount, DateTime created)
        {
            Id = DateTime.Now.Ticks;
            Reference = reference;
            Customer = customer;
            Amount = amount;
            Created = created;
            Expected = created + TimeSpan.FromDays(30);
        }

        public long Id { get; set; }

        [Required(ErrorMessage = "Invoice reference is required")]
        public string Reference { get; set; }

        [Required(ErrorMessage = "Customer reference is required")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Amount reference is required")]
        public decimal Amount { get; set; }

        public decimal Paid { get; private set; } = 0m;

        [Required(ErrorMessage = "Created reference is required")]
        public DateTime Created { get; set; }

        public DateTime Expected { get; set; }
        public DateTime? LastPayment { get; private set; } = null;

        public bool IsPaid => Paid == Amount;
        public bool IsLate => Expected < DateTime.Now;

        public void RegisterPayment(DateTime when, decimal howMany)
        {
            if (when < Created)
            {
                throw new ArgumentOutOfRangeException("Cannot pay before the invoice creation");
            }
            LastPayment = when;
            if (Amount - Paid < howMany)
            {
                throw new ArgumentOutOfRangeException("Cannot pay over the due amount");
            }
            Paid += howMany;
        }
    }
}