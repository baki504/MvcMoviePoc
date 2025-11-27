using MvcMoviePoc.Models;

namespace MvcMoviePoc.Logic
{
    // Simple business logic example for pricing.
    public class MoviePricingLogic
    {
        // Calculates discounted price based on genre and rating.
        // - Family genre gets 10% off
        // - Rating "PG" gets extra 5% off
        // - Minimum price floor at 1.00
        public decimal CalculateDiscountedPrice(Movie movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie));

            var price = movie.Price;
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(movie.Price), "Price cannot be negative.");

            decimal discount = 0m;
            if (string.Equals(movie.Genre, "Family", StringComparison.OrdinalIgnoreCase))
            {
                discount += 0.10m;
            }
            if (string.Equals(movie.Rating, "PG", StringComparison.OrdinalIgnoreCase))
            {
                discount += 0.05m;
            }

            var discounted = price * (1 - discount);
            if (discounted < 1.00m)
            {
                discounted = 1.00m;
            }
            return Math.Round(discounted, 2);
        }
    }
}
