using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TvShowsApp.Models
{
    public class TvShow
    {
        public TvShow()
        {
            Actors = new HashSet<Actors>();
            RentedMovies = new HashSet<RentedMovies>();
        }
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.0#}")]
        public decimal Rating { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name="Imdb Link")]
        public string ImdbUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Poster")]
        public string ImageUrl { get; set; }

        public Boolean Available { get; set; }

        public ICollection<Actors> Actors { get; set; }   //pokazhuva deka prima foreign key od actors
        public ICollection<RentedMovies> RentedMovies { get; set; }

    }
}
