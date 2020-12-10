using Invoicing.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoicing.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IBusinessData _data;

        public InvoicesController(ILogger<InvoicesController> logger, IBusinessData data)
        {
            _logger = logger;
            _data = data;
        }

        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return _data.AllInvoices;
        }

        [HttpGet("{reference}")]
        public ActionResult<Invoice> Get(string reference)
        {
            var invoice = _data.AllInvoices.Where(inv => inv.Reference == reference).FirstOrDefault();

            if (invoice != null)
            {
                return invoice;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{long:id}")]
        public ActionResult<Invoice> GetById(long id)
        {
            var invoice = _data.AllInvoices.Where(inv => inv.Id == id).FirstOrDefault();

            if (invoice != null)
            {
                return invoice;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("dashboard/{value}")]
        public ActionResult<decimal> GetDashboard(string value)
        {
            if (value == "outstanding")
            {
                decimal outstanding = _data.AllInvoices.Sum(invoice => invoice.Amount - invoice.Paid);
                return outstanding;
            }
            else if (value == "sales")
            {
                decimal sales = _data.AllInvoices.Sum(invoice => invoice.Amount);
                return sales;
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }

        [HttpPost]
        public ActionResult<Invoice> Post([FromQuery] Invoice newInvoice)
        {
            if (ModelState.IsValid)
            {
                // TODO : Ajouter la nouvelle facture à la collection
                _data.Add(newInvoice);
                return Created($"invoices/{newInvoice.Reference}", newInvoice);
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
    }
}