using System;

namespace MvcMoviePoc.ViewModels
{
    public class MovieItemViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public DateTime ReleaseDate { get; init; }
        public string? Genre { get; init; }
        public string Rating { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}