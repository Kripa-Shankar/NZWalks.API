using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext:DbContext 
    {
        //ctor for creating constructor for the class
        public NZWalksDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        //A DbSet is a property of DbContext class that represents a collection of entities in the Database 
        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

    }
}
