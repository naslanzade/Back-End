using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace OneSoundApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Singer > Singers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<PlaylistImage> PlaylistImages { get; set; }

        public DbSet<Podcast> Podcast { get; set; }
        public DbSet<PodcastImage> PodcastImage { get; set; }
        public DbSet<Record> Record { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Setting>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Event>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Slider>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Singer>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Song>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Album>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Author>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Category>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Blog>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Advert>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Podcast>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<PodcastImage>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Record>().HasQueryFilter(m => !m.SoftDelete);


            modelBuilder.Entity<Album>()
                       .HasMany(e => e.Song)
                       .WithOne(e => e.Album)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
