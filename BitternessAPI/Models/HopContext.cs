using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BitternessAPI.Models
{
    public class HopContext : DbContext
    {
        public HopContext(DbContextOptions<HopContext> options) : base(options)
        {

        }

        public DbSet<Hop> Hops { get; set; }
    }
}
