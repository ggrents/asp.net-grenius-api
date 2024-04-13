using grenius_api.Infrastructure.Configurations;
using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Infrastructure.Database
{
    public class GreniusContext : DbContext
    {
        private readonly string? _connectionString;
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Feature> Features{ get; set; }
        public GreniusContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlbumConfiguration());
            modelBuilder.ApplyConfiguration(new ArtistConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureConfiguration());
            modelBuilder.ApplyConfiguration(new SongConfiguration());
        }

    }
}
