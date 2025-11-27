using MvcMoviePoc.Logic;
using MvcMoviePoc.Models;
using MvcMoviePoc.ViewModels;

namespace MvcMoviePoc.Factories
{
    public class MovieViewModelFactory
    {
        private readonly MoviePricingLogic _pricing = new();

        public MovieItemViewModel CreateItem(Movie movie)
            => new MovieItemViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Rating = movie.Rating,
                Price = movie.Price,
                DiscountedPrice = _pricing.CalculateDiscountedPrice(movie)
            };

        public MovieListViewModel CreateList(IReadOnlyList<Movie> movies, string? titleFilter, string? genreFilter, string? ratingFilter)
            => new MovieListViewModel
            {
                Items = movies.Select(CreateItem).ToList(),
                TitleFilter = titleFilter,
                GenreFilter = genreFilter,
                RatingFilter = ratingFilter,
                TotalCount = movies.Count
            };
    }
}