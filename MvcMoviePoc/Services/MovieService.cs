using System.Collections.Generic;
using System.Threading.Tasks;
using MvcMoviePoc.Factories;
using MvcMoviePoc.Models;
using MvcMoviePoc.Repositories;
using MvcMoviePoc.ViewModels;

namespace MvcMoviePoc.Services
{
    public class MovieService
    {
        private readonly MovieRepository _repository; // ???
        private readonly MovieViewModelFactory _factory;

        public MovieService(MovieRepository repository, MovieViewModelFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public async Task<MovieListViewModel> GetListAsync(string? titleFilter, string? genreFilter, string? ratingFilter)
        {
            IReadOnlyList<Movie> movies = await _repository.GetAllAsync(titleFilter, genreFilter, ratingFilter);
            MovieListViewModel vm = _factory.CreateList(movies, titleFilter, genreFilter, ratingFilter);
            return vm;
        }

        public async Task<MovieItemViewModel?> GetAsync(int id)
        {
            Movie? movie = await _repository.GetByIdAsync(id);
            if (movie is null)
            {
                return null;
            }

            MovieItemViewModel vm = _factory.CreateItem(movie);
            return vm;
        }

        public async Task<int> CreateAsync(MovieItemViewModel input)
        {
            Movie movie = new Movie
            {
                Title = input.Title,
                ReleaseDate = input.ReleaseDate,
                Genre = input.Genre,
                Rating = input.Rating,
                Price = input.Price
            };

            await _repository.AddAsync(movie);
            return movie.Id;
        }

        public async Task<bool> UpdateAsync(MovieItemViewModel input)
        {
            bool exists = await _repository.ExistsAsync(input.Id);
            if (!exists)
            {
                return false;
            }

            Movie movie = new Movie
            {
                Id = input.Id,
                Title = input.Title,
                ReleaseDate = input.ReleaseDate,
                Genre = input.Genre,
                Rating = input.Rating,
                Price = input.Price
            };

            await _repository.UpdateAsync(movie);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}