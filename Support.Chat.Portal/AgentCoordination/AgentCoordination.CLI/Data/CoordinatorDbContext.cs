using Microsoft.EntityFrameworkCore;
using Support.Chat.Portal.Common.Enums;
using Support.Chat.Portal.Common.Models;

namespace AgentCoordination.CLI.Data
{
    public class CoordinatorDbContext : DbContext
    {
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Seniority> Seniorities { get; set; }
        public DbSet<Agent> Agents { get; set; }

        public string DbPath { get; private set; }

        public CoordinatorDbContext()
        {
            DbPath = "CoordinatorDb.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Seniority>()
            .HasData(
                new Seniority { Id = 1, Efficiency = 0.4, Name = "Junior" },
                new Seniority { Id = 2, Efficiency = 0.6, Name = "MidLevel" },
                new Seniority { Id = 3, Efficiency = 0.8, Name = "Senior" },
                new Seniority { Id = 4, Efficiency = 0.5, Name = "TeamLead" }
                );


            modelBuilder.Entity<Team>()
            .HasData(
                new Team { Id = 1, Name = "Team A", IsOverflow = true, Shift = Shift.OfficeTime },
                new Team { Id = 2, Name = "Team B", IsOverflow = false, Shift = Shift.Evening },
                new Team { Id = 3, Name = "Team C", IsOverflow = false, Shift = Shift.Night },
                new Team { Id = 4, Name = "Overflow", IsOverflow = false, Shift = Shift.None }
                );


            modelBuilder.Entity<Agent>()
            .HasData(
                new Agent { Id = 1, Name = "Robert", SeniorityId = 1, TeamId = 1 },
                new Agent { Id = 2, Name = "Chris", SeniorityId = 4, TeamId = 1 },
                new Agent { Id = 3, Name = "Josh", SeniorityId = 2, TeamId = 1 },
                new Agent { Id = 4, Name = "Evans", SeniorityId = 2, TeamId = 1 },

                new Agent { Id = 5, Name = "Scarlett", SeniorityId = 3, TeamId = 2 },
                new Agent { Id = 6, Name = "Tom", SeniorityId = 2, TeamId = 2 },
                new Agent { Id = 7, Name = "Mark", SeniorityId = 1, TeamId = 2 },
                new Agent { Id = 8, Name = "Elizabeth", SeniorityId = 1, TeamId = 2 },

                new Agent { Id = 9, Name = "Paul", SeniorityId = 2, TeamId = 3 },
                new Agent { Id = 10, Name = "Benedict", SeniorityId = 2, TeamId = 3 },

                new Agent { Id = 11, Name = "Peter", SeniorityId = 1, TeamId = 4 },
                new Agent { Id = 12, Name = "Bradly", SeniorityId = 1, TeamId = 4 },
                new Agent { Id = 13, Name = "Idris", SeniorityId = 1, TeamId = 4 },
                new Agent { Id = 14, Name = "Ross", SeniorityId = 1, TeamId = 4 },
                new Agent { Id = 15, Name = "Ariana", SeniorityId = 1, TeamId = 4 },
                new Agent { Id = 16, Name = "Tiffany", SeniorityId = 1, TeamId = 4 }
             );

                // base.OnModelCreating(modelBuilder);

            }

    }
}
