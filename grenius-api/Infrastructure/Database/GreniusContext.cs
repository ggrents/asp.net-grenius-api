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
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Role> Roles{ get; set; }
        public DbSet<UserRole> UserRoles{ get; set; }
        public DbSet<Lyrics> Lyrics{ get; set; }
        public DbSet<Annotation> Annotations { get; set; }
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
            modelBuilder.ApplyConfiguration(new LyricsConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AnnotationConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerConfiguration());
        }

    }
}
