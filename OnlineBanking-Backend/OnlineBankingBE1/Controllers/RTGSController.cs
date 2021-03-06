using Microsoft.AspNetCore.Mvc;
using OnlineBankingBE1.Models;
using OnlineBankingBE1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingBE1.Controllers
{
    [Route("api/Rtgs")]
    [ApiController]
    public class RTGSController : Controller
    {
        private readonly IDataRepository<RTGSPay> _context;
        private readonly IDataRepository<AccountStatement> _context1;

        public RTGSController(IDataRepository<RTGSPay> context, IDataRepository<AccountStatement> context1)
        {
            _context = context;
            _context1 = context1;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<RTGSPay> customers = _context.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IEnumerable<RTGSPay> customers = _context.GetByAccount(id);
            return Ok(customers);
        }


        [HttpPost]
        public IActionResult Post(RTGSPay customer)
        {
            AccountStatement acc1 = _context1.Get(customer.FromAccount);
            AccountStatement acc2 = _context1.Get(customer.ToAccount);
            if (acc1 != null && acc2 != null)
            {
                if (acc1.Balance >= customer.Amount)
                {
                    acc1.Balance = acc1.Balance - customer.Amount;
                    acc2.Balance = acc2.Balance + customer.Amount;
                    _context1.Update(_context1.Get(acc1.AccountNumber), acc1);
                    _context1.Update(_context1.Get(acc2.AccountNumber), acc2);
                    _context.Add(customer);
                    return Ok(customer);
                }
                else
                {
                    return BadRequest("Please Check Your Balance!");
                }
            }
            if (customer == null)
            {
                return BadRequest("Account Number Not Found.");
            }

            _context.Add(customer);
            //return NoContent();
            return Ok(customer);
        }



        // PUT: api/customer/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, RTGSPay customer)
        {
            RTGSPay customerToUpdate = _context.Get(id);
            if (customerToUpdate == null)
            {
                return NotFound("The customer record couldn't be found.");
            }

            _context.Update(customerToUpdate, customer);
            return Ok(customerToUpdate);
            //return NoContent();
        }

        // DELETE: api/customer/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            RTGSPay customer = _context.Get(id);
            if (customer == null)
            {
                return NotFound("The customer record couldn't be found.");
            }

            _context.Delete(customer.RTGSId);
            return NoContent();
        }
    }
}
