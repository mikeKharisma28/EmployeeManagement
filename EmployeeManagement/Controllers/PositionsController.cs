using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement;
using EmployeeManagement.Models;
using EmployeeManagement.DTOs.Position;
using EmployeeManagement.Extensions;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PositionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            return await _context.Positions.ToListAsync();
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(Guid id)
        {
            var position = await _context.Positions.FindAsync(id);

            if (position == null)
            {
                return NotFound();
            }

            return position;
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(Guid id, EditDto positionEditDto)
        {
            var existing = await _context.Positions.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            else
            {
                //_context.Entry(existing).State = EntityState.Modified;

                existing.Name = positionEditDto.Name;
                existing.Description = positionEditDto.Description;
                existing.IsActive = positionEditDto.IsActive;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(id))
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

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(AddDto positionAddDto)
        {
            var position = new Position();
            position.Name = positionAddDto.Name;
            position.Description = positionAddDto.Description;
            position.IsActive = positionAddDto.IsActive;

            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPosition", new { id = position.Id }, position);
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(Guid id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            _context.SoftDelete(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PositionExists(Guid id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
    }
}
