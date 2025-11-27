using Microsoft.EntityFrameworkCore;
using MvcMoviePoc.Data;
using MvcMoviePoc.Models;

namespace MvcMoviePoc.Repositories
{
    public class MovieRepository
    {
        private readonly MvcMoviePocContext _context;

        public MovieRepository(MvcMoviePocContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Movie>> GetAllAsync(string? titleFilter, string? genreFilter, string? ratingFilter)
        {
            IQueryable<Movie> query = _context.Movie.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(titleFilter))
            {
                query = query.Where(m => m.Title.Contains(titleFilter));
            }

            if (!string.IsNullOrWhiteSpace(genreFilter))
            {
                query = query.Where(m => m.Genre == genreFilter);
            }

            if (!string.IsNullOrWhiteSpace(ratingFilter))
            {
                query = query.Where(m => m.Rating == ratingFilter);
            }

            List<Movie> result = await query
                .OrderBy(m => m.Title)
                .ToListAsync();

            return result;
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            Movie? movie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            return movie;
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Movie.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movie.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Movie? movie = await _context.Movie.FindAsync(id);
            if (movie is not null)
            {
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            bool exists = await _context.Movie.AnyAsync(e => e.Id == id);
            return exists;
        }
    }
}