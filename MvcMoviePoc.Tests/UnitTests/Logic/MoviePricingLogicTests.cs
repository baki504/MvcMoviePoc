using System;
using MvcMoviePoc.Logic;
using MvcMoviePoc.Models;
using Xunit;

namespace MvcMoviePoc.Tests.UnitTests.Logic
{
    public class MoviePricingLogicTests
    {
        [Fact]
        public void CalculateDiscountedPrice_FamilyGenreGets10Percent()
        {
            var movie = new Movie { Price = 20m, Genre = "Family", Rating = "G" };
            var sut = new MoviePricingLogic();

            var result = sut.CalculateDiscountedPrice(movie);

            Assert.Equal(18m, result);
        }

        [Fact]
        public void CalculateDiscountedPrice_PgRatingGetsAdditional5Percent()
        {
            var movie = new Movie { Price = 20m, Genre = "Action", Rating = "PG" };
            var sut = new MoviePricingLogic();

            var result = sut.CalculateDiscountedPrice(movie);

            Assert.Equal(19m, result);
        }

        [Fact]
        public void CalculateDiscountedPrice_FamilyAndPgGetsTotal15Percent()
        {
            var movie = new Movie { Price = 100m, Genre = "Family", Rating = "PG" };
            var sut = new MoviePricingLogic();

            var result = sut.CalculateDiscountedPrice(movie);

            Assert.Equal(85m, result);
        }

        [Fact]
        public void CalculateDiscountedPrice_DoesNotGoBelowOneDollar()
        {
            var movie = new Movie { Price = 0.5m, Genre = "Family", Rating = "PG" };
            var sut = new MoviePricingLogic();

            var result = sut.CalculateDiscountedPrice(movie);

            Assert.Equal(1.00m, result);
        }

        [Fact]
        public void CalculateDiscountedPrice_ThrowsOnNegativePrice()
        {
            var movie = new Movie { Price = -1m };
            var sut = new MoviePricingLogic();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.CalculateDiscountedPrice(movie));
        }

        [Fact]
        public void CalculateDiscountedPrice_ThrowsOnNullMovie()
        {
            var sut = new MoviePricingLogic();

            Assert.Throws<ArgumentNullException>(() => sut.CalculateDiscountedPrice(null!));
        }
    }
}
