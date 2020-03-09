using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvShowsApp.Models
{
    public class Users
    {
        public Users()
        {
            RentedMovies = new HashSet<RentedMovies>(); // Allows Icollections and other methods
        }
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public ICollection<RentedMovies> RentedMovies { get; set; } //  foreign key

    }
}
