using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMoviePoc.Data
{
    public class MvcMoviePocContext : DbContext
    {
        public MvcMoviePocContext (DbContextOptions<MvcMoviePocContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
