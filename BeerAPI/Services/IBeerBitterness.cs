using BeerAPI.Services.Models;
using System.Collections.Generic;

namespace BeerAPI.Services
{
    public interface IBeerBitterness
    {
        double Bitterness(double volume, List<HopModel> hops);
    }
}
