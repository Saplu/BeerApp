using BitternessAPI.Models;
using IbuCalculations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitternessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopsController : ControllerBase
    {
        private readonly HopContext _context;

        public HopsController(HopContext context)
        {
            _context = context;
        }

        // GET: api/Hops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Hop>>> GetHops()
        {
            return await _context.Hops.ToListAsync();
        }

        // GET: api/Hops/5
        [HttpGet("{volume}")]
        public async Task<double> GetBitterness(int volume)
        {
            var hops = ConvertContextHopsToHops();
            var calculator = new BeerBitternessCalculator(volume, hops);
            return calculator.Bitterness();
        }

        //// PUT: api/Hops/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutHop(long id, Hop hop)
        //{
        //    if (id != hop.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(hop).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!HopExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Hops
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Models.Hop>>> PostHop(Models.Hop hop)
        {
            _context.Hops.Add(hop);
            await _context.SaveChangesAsync();

            return _context.Hops;
        }

        // DELETE: api/Hops/5
        [HttpDelete]
        public async Task<StatusCodeResult> DeleteHop()
        {
            foreach(var item in _context.Hops)
            {
                _context.Remove(item);
            }
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool HopExists(long id)
        {
            return _context.Hops.Any(e => e.Id == id);
        }

        private List<IbuCalculations.Models.Hop> ConvertContextHopsToHops()
        {
            var hops = new List<IbuCalculations.Models.Hop>();
            foreach(var hop in _context.Hops)
            {
                var newHop = new IbuCalculations.Models.Hop(hop.Name, hop.Weight, hop.Alpha, (int)hop.BoilingTime);
                hops.Add(newHop);
            }
            return hops;
        }
    }
}
