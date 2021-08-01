using Freewheel.DBContext;
using Freewheel.Helper;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freewheel.Repositories
{
    public class MovieRepository : IMovie
    {
        private AppDbContext _addDbContext = null;

        public MovieRepository(AppDbContext appDbContext)
        {
            _addDbContext = appDbContext;
        }

        public IEnumerable<Movie> GetMovies(string title = null, DateTime? releasedDate = null, string generes = null)
        {
            return _addDbContext.Movies.Where(s => s.Generes == generes
        || (releasedDate.HasValue
            && s.ReleasedDate.Year == releasedDate.Value.Year)
        || s.Title == title).OrderBy(s => s.Title);
        }

        public IEnumerable<MovieInfo> GetTop5AverageRatedMovie()
        {
            var averageRatedMovie = _addDbContext.Movies.Include(r => r.UserRatings).ToList().GroupBy(s => new
            {
                movieId = s.Id,
                avergaeRating = Math.Round(s.UserRatings.Select(s => s.Rating).Average())
            }).Where(s => s.Key.avergaeRating >= 3.0).OrderByDescending(s => s.Key.avergaeRating).Take(5).ToDictionary(s => s.Key.movieId, s=>s.Key.avergaeRating);
            return _addDbContext.Movies.Where(s => averageRatedMovie.Keys.Contains(s.Id)).Select(r => new MovieInfo
            {
                Id= r.Id,
                Title= r.Title,
                Genres=r.Generes,
                AverageRating = averageRatedMovie[r.Id],
                RunningTime = r.ReleasedDate.ToShortTimeString(),
                YearOfRelease = r.ReleasedDate.Year
            });
        }

        public IEnumerable<MovieInfo> GetTop5RatedMovieByUser(int userId)
        {
            var user = _addDbContext.Users.SingleOrDefault(s => s.Id == userId);
            if (user != null)
            {
                var averageRatedMovie = _addDbContext.Movies.GroupBy(s => new
                {
                    movieId = s.Id,
                    avergaeRating = Math.Round(s.UserRatings.Where(s => s.User.Id == userId).Select(s => s.Rating).Average())
                }).Where(s => s.Key.avergaeRating >= 3.0).OrderByDescending(s => s.Key.avergaeRating).Take(5).ToDictionary(s => s.Key.movieId, s => s.Key.avergaeRating);
                return _addDbContext.Movies.Where(s => averageRatedMovie.Keys.Contains(s.Id)).Select(r => new MovieInfo
                {
                    Id = r.Id,
                    Title = r.Title,
                    Genres = r.Generes,
                    AverageRating = averageRatedMovie[r.Id],
                    RunningTime = r.ReleasedDate.ToShortTimeString(),
                    YearOfRelease = r.ReleasedDate.Year
                });
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public Movie UpsertMovieRatings(int userId, string movieTitle, UserRating userRating)
        {
            var user = _addDbContext.Users.SingleOrDefault(s => s.Id == userId);
            if (user != null)
            {
                var movie = _addDbContext.Movies.Where(s => s.Title == movieTitle).SingleOrDefault();
                var rating = movie.UserRatings.SingleOrDefault(s => s.User.Id == userId);
                rating = rating ?? new UserRating();
                rating.Rating = userRating.Rating;
                rating.User = new User()
                {
                    Id = userId
                };
                movie.UserRatings.Add(userRating);
                _addDbContext.SaveChanges();
                return movie;
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
    }
}
