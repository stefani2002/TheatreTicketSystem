using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.Domain.Entities;

namespace TheatreTicketSystem.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Play> Plays { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Play>().HasData(
        new Play
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Title = "Хамлет",
            Description = "Класична трагедија од Вилијам Шекспир",
            Genre = "Драма",
            DurationMinutes = 180
        },
        new Play
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "Чекајќи го Годо",
            Description = "Апсурдна драма од Самуел Бекет",
            Genre = "Драма",
            DurationMinutes = 120
        }
    );
            modelBuilder.Entity<Show>().HasData(
        new Show
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            ShowTime = DateTime.Now.AddDays(1),
            Price = 500,
            TotalSeats = 100,
            AvailableSeats = 100,
            PlayId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        },
        new Show
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ShowTime = DateTime.Now.AddDays(2),
            Price = 400,
            TotalSeats = 80,
            AvailableSeats = 80,
            PlayId = Guid.Parse("22222222-2222-2222-2222-222222222222")
        }
    );


        }
    }
}
