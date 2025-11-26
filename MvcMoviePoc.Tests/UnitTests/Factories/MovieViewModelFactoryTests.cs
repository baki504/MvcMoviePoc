using System;
using System.Collections.Generic;
using MvcMoviePoc.Factories;
using MvcMoviePoc.Models;
using MvcMoviePoc.ViewModels;
using Xunit;

namespace MvcMoviePoc.Tests.UnitTests.Factories
{
    public class MovieViewModelFactoryTests
    {
        private readonly MovieViewModelFactory _factory = new();

        [Fact]
        public void CreateItem_Maps_All_Properties()
        {
            var movie = new Movie
            {
                Id = 42,
                Title = "The Test",
                ReleaseDate = new DateTime(2020, 1, 2),
                Genre = "Sci-Fi",
                Rating = "PG-13",
                Price = 12.34M
            };

            MovieItemViewModel vm = _factory.CreateItem(movie);

            Assert.Equal(movie.Id, vm.Id);
            Assert.Equal(movie.Title, vm.Title);
            Assert.Equal(movie.ReleaseDate, vm.ReleaseDate);
            Assert.Equal(movie.Genre, vm.Genre);
            Assert.Equal(movie.Rating, vm.Rating);
            Assert.Equal(movie.Price, vm.Price);
        }

        [Fact]
        public void CreateList_Maps_Items_Filters_And_TotalCount()
        {
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "A", ReleaseDate = new DateTime(2001,1,1), Genre = "Comedy", Rating = "G", Price = 1.23M },
                new Movie { Id = 2, Title = "B", ReleaseDate = new DateTime(2002,2,2), Genre = "Action", Rating = "PG", Price = 4.56M }
            };
            string? titleFilter = "A";
            string? genreFilter = "Comedy";
            string? ratingFilter = "G";

            MovieListViewModel vm = _factory.CreateList(movies, titleFilter, genreFilter, ratingFilter);

            Assert.Equal(movies.Count, vm.TotalCount);
            Assert.Equal(titleFilter, vm.TitleFilter);
            Assert.Equal(genreFilter, vm.GenreFilter);
            Assert.Equal(ratingFilter, vm.RatingFilter);

            Assert.Collection(vm.Items,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("A", item.Title);
                    Assert.Equal(new DateTime(2001,1,1), item.ReleaseDate);
                    Assert.Equal("Comedy", item.Genre);
                    Assert.Equal("G", item.Rating);
                    Assert.Equal(1.23M, item.Price);
                },
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal("B", item.Title);
                    Assert.Equal(new DateTime(2002,2,2), item.ReleaseDate);
                    Assert.Equal("Action", item.Genre);
                    Assert.Equal("PG", item.Rating);
                    Assert.Equal(4.56M, item.Price);
                }
            );
        }

        [Fact]
        public void CreateList_With_Empty_Movies_Sets_Empty_Items_And_Count_Zero()
        {
            var movies = new List<Movie>();

            MovieListViewModel vm = _factory.CreateList(movies, null, null, null);

            Assert.Empty(vm.Items);
            Assert.Equal(0, vm.TotalCount);
            Assert.Null(vm.TitleFilter);
            Assert.Null(vm.GenreFilter);
            Assert.Null(vm.RatingFilter);
        }
    }
}
