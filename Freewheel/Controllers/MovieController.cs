using Freewheel.Helper;
using Freewheel.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

namespace Freewheel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private IMovie _movieRepository;

        public MovieController(ILogger<MovieController> logger, IMovie movie)
        {
            _logger = logger;
            _movieRepository = movie;
        }

        /// <summary>
        /// Get the list of movie
        /// </summary>
        /// <param name="title">Filter by title</param>
        /// <param name="releasedDate">Filter by released date</param>
        /// <param name="genere">Filter by genere</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Movie>))]
        public ActionResult Get([FromQuery] string title = null, [FromQuery] DateTime? releasedDate = null, [FromQuery] string genere = null)
        {
            if (!Helper.FreeWheelHelper.ValidateFilterCriteria(title, releasedDate, genere))
            {
                return BadRequest("no criteria is given, must give at least one");
            }
            else
            {
                var movieList = _movieRepository.GetMovies(title, releasedDate, genere);
                if (movieList.Count() > 0)
                {
                    return Ok(movieList);
                }
                return NotFound();
            }
        }

        /// <summary>
        /// Get top 5 average rated movie cross the library
        /// </summary>
        /// <returns></returns>
        [Route("Top5Movie")]
        [ResponseType(typeof(IEnumerable<Models.Movie>))]
        [HttpGet]
        public ActionResult GetAverageRatingsMovie()
        {
            var movieList = _movieRepository.GetTop5AverageRatedMovie();
            if (movieList.Count() > 0)
            {
                return Ok(movieList);
            }
            return NotFound();
        }

        /// <summary>
        /// Get Top 5 highest rated movie for specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Top5Movie/{id}")]
        [ResponseType(typeof(IEnumerable<Models.Movie>))]
        [HttpGet]
        public ActionResult GetAverageRatingsMovie(int id)
        {
            try
            {
                var movieList = _movieRepository.GetTop5RatedMovieByUser(id);
                if (movieList.Count() > 0)
                {
                    return Ok(movieList);
                }
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }


        }
        /// <summary>
        /// Add or Update ratings of the selected movie
        /// </summary>
        /// <param name="id">User Identifier</param>
        /// <param name="movieTitle">Title of the Movie</param>
        /// <param name="userRating">UserRating Params</param>
        /// <returns></returns>
        [Route("Top5Movie/{id}")]
        [ResponseType(typeof(Models.Movie))]
        [HttpPut]
        public ActionResult AddOrUpdateUserRatings(int id, [FromQuery] string movieTitle, [FromBody] UserRating userRating)
        {
            try
            {
                var movie = _movieRepository.UpsertMovieRatings(id, movieTitle, userRating);
                if (movie != null)
                {
                    return Ok(movie);
                }
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
