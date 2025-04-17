using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using EmployeeManagement.DTOs.Employee;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using EmployeeManagement.Services;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly IUserAccountServices _userAccountServices;
        private readonly ExternalAPIServices _externalAPIServices;

        public EmployeesController(AppDbContext context, IDistributedCache cache, IUserAccountServices userAccountServices, ExternalAPIServices externalAPIServices)
        {
            _context = context;
            _cache = cache;
            _userAccountServices = userAccountServices;
            _externalAPIServices = externalAPIServices;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            string cacheKey = $"Employee_{id}";
            var cachedItem = await _cache.GetStringAsync(cacheKey);
            if (cachedItem != null)
            {
                Console.WriteLine(cachedItem);
                return JsonConvert.DeserializeObject<Employee>(cachedItem);
            }
            else
            {
                var employee = await _context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    await _cache.SetStringAsync(cacheKey,
                        JsonConvert.SerializeObject(employee),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // remove data after a fixed time
                            SlidingExpiration = TimeSpan.FromMinutes(2) // reset expiration every time data is accessed
                        });
                }

                return employee;
            }
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, UpdateDto updateEmployeeDto)
        {
            var existing = await _context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (existing == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    existing.FullName = updateEmployeeDto.FullName;
                    existing.EmployeeNumber = updateEmployeeDto.EmployeeNumber;
                    existing.PhoneNumber = updateEmployeeDto.PhoneNumber;
                    existing.Address = updateEmployeeDto.Address;
                    existing.PositionId = updateEmployeeDto.PositionId;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(InsertDto insertEmployeeDto)
        {
            try
            {
                var employee = new Employee();

                employee.FullName = insertEmployeeDto.FullName;
                employee.EmployeeNumber = insertEmployeeDto.EmployeeNumber;
                employee.PhoneNumber = insertEmployeeDto.PhoneNumber;
                employee.Address = insertEmployeeDto.Address;
                employee.PositionId = insertEmployeeDto.PositionId;

                // upload image
                var urlResult = await _externalAPIServices.UploadImage(insertEmployeeDto.FilePhoto);
                employee.PhotoUrl = urlResult;

                // add user data
                var isSucceeded = await _userAccountServices.Register(insertEmployeeDto);

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
