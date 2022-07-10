using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JopPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JopPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenericCollectionController<T> : ControllerBase where T : class
    {
        private readonly JobPortal2Context _context;

        public GenericCollectionController(JobPortal2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> ListOfDetails()
        {
            return await _context.Set<T>().ToListAsync();
        }

        //[HttpGet("{Id}")]        
        //public async Task<ActionResult> DetailsOfParticular(int Id)
        //{
        //    return Ok(_context.Set<T>().FindAsync(Id));
            
        //}


        [HttpPost]
        public async Task<ActionResult<T>> PostDetails(T Details)
        {
            _context.Set<T>().Add(Details);
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Put")]
        public async Task<IActionResult> PutDetails(T Details)
        {           

            _context.Entry(Details).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllUserDetails(int id)
        {
            var Details = await _context.Set<T>().FindAsync(id);
            if (Details == null)
            {
                return NotFound();
            }

            _context.Set<T>().Remove(Details);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
