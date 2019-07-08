using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data.Models;

namespace OnLineVideotech.Data
{
    public class OnLineVideotechDbContext : IdentityDbContext<User>
    {
        public OnLineVideotechDbContext(DbContextOptions<OnLineVideotechDbContext> options)
            : base(options)
        {
        }

        public DbSet<History> Histories { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<GenreMovie> GenreMovies { get; set; }

        public DbSet<UserMoneyBalance> UserMoneyBalance { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<GenreMovie>()
                .HasKey(x => new { x.GenreId, x.MovieId });

            builder
                .Entity<GenreMovie>()
                .HasOne(gm => gm.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(gm => gm.GenreId);

            builder
                .Entity<GenreMovie>()
                .HasOne(gm => gm.Movie)
                .WithMany(g => g.Genres)
                .HasForeignKey(gm => gm.MovieId);

            builder
                .Entity<Price>()
                .HasKey(pr => new { pr.Id, pr.MovieId, pr.RoleId });

            builder
                .Entity<Price>()
                .HasOne(m => m.Movie)
                .WithMany(r => r.Prices)
                .HasForeignKey(m => m.MovieId);

            builder
               .Entity<Price>()
               .HasOne(r => r.Role)
               .WithMany(m => m.Movies)
               .HasForeignKey(r => r.RoleId);

            builder
                .Entity<User>()
                .HasOne(u => u.Balance)
                .WithOne(b => b.User)
                .HasForeignKey<UserMoneyBalance>(ub => ub.UserId);

            builder
                .Entity<History>()
                .HasKey(pr => new { pr.Id, pr.CustomerId, pr.MovieId });

            builder
               .Entity<History>()
               .HasOne(r => r.Movie)
               .WithMany(m => m.Histories)
               .HasForeignKey(r => r.MovieId);

            builder
               .Entity<History>()
               .HasOne(r => r.Customer)
               .WithMany(m => m.Histories)
               .HasForeignKey(r => r.CustomerId);

            builder
                .Entity<Comment>()
                .HasKey(com => new { com.Id, com.CustomerId , com.MovieId});

            builder
               .Entity<Comment>()
               .HasOne(c => c.Customer)
               .WithMany(m => m.Comments)
               .HasForeignKey(c => c.CustomerId);

            builder
               .Entity<Comment>()
               .HasOne(c => c.Movie)
               .WithMany(m => m.Comments)
               .HasForeignKey(c => c.MovieId);

            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        }
    }
}