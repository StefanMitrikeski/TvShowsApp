using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvShowsApp.Models;

namespace TvShowsApp.Models
{
    public  class TvShowsContext : DbContext
    {
        public TvShowsContext (DbContextOptions<TvShowsContext> options)
            : base(options)
        {
        }

        public DbSet<TvShowsApp.Models.TvShow> TvShow { get; set; }

        public DbSet<TvShowsApp.Models.Actors> Actors { get; set; }

        public DbSet<TvShowsApp.Models.Users> Users { get; set; }

        public DbSet<TvShowsApp.Models.RentedMovies> RentedMovies { get; set; }

       

    }
}
