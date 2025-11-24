using Microsoft.EntityFrameworkCore;

namespace MvcMoviePoc.Data
{
    public class MvcMoviePocContext(DbContextOptions<MvcMoviePocContext> options) : DbContext(options)
    {
        public DbSet<MvcMoviePoc.Models.Movie> Movie { get; set; } = default!;
    }
}
