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
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    //[Authorize(Policy = "SuperAdminPolicy")]
    [Route("api/position")]
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
        public async Task<ActionResult<IEnumerable<ViewDto>>> GetPositions()
        {
            List<ViewDto> positionsDto = new List<ViewDto>();
            var existingData = await _context.Positions.ToListAsync();
            foreach (var position in existingData)
            {
                ViewDto positionDto = new ViewDto();
                positionDto.Name = position.Name;
                positionDto.Description = position.Description;
                positionDto.IsActive = position.IsActive;
                positionsDto.Add(positionDto);
            }

            return positionsDto;
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViewDto>> GetPosition(Guid id)
        {
            var position = await _context.Positions.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (position == null)
            {
                return NotFound();
            }
            else
            {
                ViewDto positionDto = new ViewDto();
                positionDto.Name = position.Name;
                positionDto.Description = position.Description;
                positionDto.IsActive = position.IsActive;

                return positionDto;
            }
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(Guid id, UpdateDto updatePositionDto)
        {
            var existing = await _context.Positions.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (existing == null)
            {
                return NotFound();
            }
            else
            {
                //_context.Entry(existing).State = EntityState.Modified;

                existing.Name = updatePositionDto.Name;
                existing.Description = updatePositionDto.Description;
                existing.IsActive = updatePositionDto.IsActive;

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
        public async Task<ActionResult<Position>> PostPosition(InsertDto insertPositionDto)
        {
            var position = new Position();
            position.Name = insertPositionDto.Name;
            position.Description = insertPositionDto.Description;
            position.IsActive = insertPositionDto.IsActive;

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
            else
            {
                _context.SoftDelete(position);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }

        private bool PositionExists(Guid id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
    }
}
