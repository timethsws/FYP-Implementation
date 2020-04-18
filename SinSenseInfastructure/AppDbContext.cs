using System;
using Microsoft.EntityFrameworkCore;
using SinSenseCore.Entities;

namespace SinSenseInfastructure
{
    /// <summary>
    /// Application Data Context
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initialises a new instance of <see cref="SinSenseInfastructure.AppDbContext"/>
        /// </summary>
        /// <param name="options">Options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Words db set
        /// </summary>
        public DbSet<Word> Words { get; set; }

        /// <summary>
        /// Word relations db set
        /// </summary>
        public DbSet<WordRelation> WordRelations { get; set; }

        /// <summary>
        /// fluent db configuration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships
            modelBuilder.Entity<WordRelation>().HasOne(wr => wr.ToWord);
            modelBuilder.Entity<WordRelation>().HasOne(wr => wr.FromWord).WithMany(w => w.Relations);

            // TODO : Db Optimaisations
            modelBuilder.Entity<Word>().HasIndex(w => w.Language);
            modelBuilder.Entity<Word>().HasIndex(w => w.Text);

            modelBuilder.Entity<WordRelation>().HasIndex(r => r.Type);
            modelBuilder.Entity<WordRelation>().HasIndex(r => r.FromWordId);
            modelBuilder.Entity<WordRelation>().HasIndex(r => r.ToWordId);
        }
    }
}
