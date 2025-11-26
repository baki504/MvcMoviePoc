using System.Collections.Generic;

namespace MvcMoviePoc.ViewModels
{
    public class MovieListViewModel
    {
        public IReadOnlyList<MovieItemViewModel> Items { get; init; } = new List<MovieItemViewModel>();
        public string? TitleFilter { get; init; }
        public string? GenreFilter { get; init; }
        public string? RatingFilter { get; init; }
        public int TotalCount { get; init; }
    }
}