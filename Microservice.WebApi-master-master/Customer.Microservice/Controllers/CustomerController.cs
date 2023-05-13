using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customer.Microservice.Data;
using Customer.Microservice.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customer.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IApplicationDbContext _context;
        public CustomerController(IApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Entities.Customer customer)
        {
            customer.Password = Helper.EncryptTax(customer.Password);
            _context.Customers.Add(customer);
            await _context.SaveChanges();
            return Ok(customer.Id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var customers = await _context.Customers.ToListAsync();
            //if (customers == null) return NotFound();
            return Ok("Customer Microservice executed !");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (customer == null) return NotFound();
            _context.Customers.Remove(customer);
            await _context.SaveChanges();
            return Ok(customer.Id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Entities.Customer customerData)
        {
            var customer = _context.Customers.Where(a => a.Id == id).FirstOrDefault();

            if (customer == null) return NotFound();
            else
            {
                customer.UserName = customerData.UserName;
                customer.Name = customerData.Name;
                customer.Mobile = customerData.Mobile;
                customer.Email = customerData.Email;
                customer.Password = customerData.Password;
                await _context.SaveChanges();
                return Ok(customer.Id);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CustomerLogin(CustomerLogin request)
        {
            string pwd = Helper.EncryptTax(request.Password);
            var customer = await _context.Customers.FirstOrDefaultAsync(a => a.Email == request.Email && a.Password == pwd);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.Id);
        }
    }
}