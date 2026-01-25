using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.Domain.Entities;

namespace TheatreTicketSystem.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Play> Plays { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }


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
            modelBuilder.Entity<SiteSettings>().HasData(
    new SiteSettings
    {
        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        HeroImagePath = null
    }
);

            modelBuilder.Entity<PaymentCard>().HasData(
    new PaymentCard
    {
        Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        CardHolderName = "Test User",
        CardNumber = "4242424242424242",
        ExpMonth = 12,
        ExpYear = 2030,
        Cvv = "123",
        Balance = 50000,
        IsActive = true
    }
);


        }
    }
}
