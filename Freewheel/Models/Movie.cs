using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("freewheel_movies")]
    public class Movie
    {
        [Column("movieId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("releasedDate")]
        public DateTime ReleasedDate { get; set; }

        [Column("generes")]
        public string Generes { get; set; }

        [JsonIgnore]
        public List<UserRating> UserRatings { get; set; }
    }

    [Table("freewheel_userRatings")]
    public class UserRating
    {
        [Column("userRatingId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("userId")]
        [ForeignKey("userId")]
        public User User { get; set; }

        [Column("movieId")]
        [ForeignKey("movieId")]
        public Movie Movie { get; set; }

        [Column("rating")]
        [Range(1.0, 5.0)]
        public float Rating { get; set; }
    }


    [Table("freewheel_users")]
    public class User
    {
        [Column("userId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("fName")]
        public string FirstName { get; set; }

        [Column("lName")]
        public string LastName { get; set; }
    }

    public class MovieInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public string RunningTime { get; set; }
        public string Genres { get; set; }
        public double AverageRating { get; set; }
    }
}