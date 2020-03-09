using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvShowsApp.Models
{
    public class Actors
    {
        [Key]
        public int ActorsID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        /*[Required]
        [DataType(DataType.Url)]
        [Display(Name = "Imdb Link")]*/
        public string ImdbUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [ForeignKey("TvShowID")]
        public int TvShowID { get; set; }

        public TvShow TvShow { get; set; }

    }
}
