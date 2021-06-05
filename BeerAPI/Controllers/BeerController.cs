using BeerAPI.Services;
using BeerAPI.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BeerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeerController : ControllerBase
    {
        private readonly IBeerBitterness _beerBitterness;

        public BeerController(IBeerBitterness bitterness)
        {
            _beerBitterness = bitterness;
        }

        [HttpGet("volume/{volume}")]
        public IActionResult GetBitterness([FromRoute] double volume)
        {
            List<HopModel> hops = new List<HopModel>()
            {
                new HopModel("testi", 20, 5, 20)
            };

            var value = _beerBitterness.Bitterness(volume, hops);

            return Ok(value);
        }
        
    }
}
