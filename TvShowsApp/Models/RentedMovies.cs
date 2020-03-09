using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvShowsApp.Models
{
    public class RentedMovies
    {
        [Key]
        public int RentedMoviesId { get; set; }

        public int UserId { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public int TvShowId { get; set; }

        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Users User { get; set; }
        public TvShow TvShow { get; set; }


        /*public RentedMovies()
        {
            RentalDate = DateTime.Now;
            ReturnDate = DateTime.Now;
        }*/
        //Foreign key is created with ICollection

        //public string Status { get; set; }

        //public IList<RentedMovies>RentedMovie { get; set; }

    }
}
