using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freewheel.Repositories
{
    public interface IMovie
    {
        public IEnumerable<Models.Movie> GetMovies(string title = null, DateTime? releasedDate = null, string generes = null);

        public IEnumerable<Models.MovieInfo> GetTop5AverageRatedMovie();

        public IEnumerable<Models.MovieInfo> GetTop5RatedMovieByUser(int userId);

        public Models.Movie UpsertMovieRatings(int userId, string movieTitle, Models.UserRating userRating);
    }
}
