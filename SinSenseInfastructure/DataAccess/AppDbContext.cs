using System;
using Microsoft.EntityFrameworkCore;
using SinSense.Core.Entities;

namespace SinSense.Infastructure
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
            modelBuilder.Entity<WordRelation>().HasOne(wr => wr.FromWord).WithMany(w => w.Relations).OnDelete(DeleteBehavior.NoAction);

            // TODO : Db Optimaisations
            modelBuilder.Entity<Word>().HasKey(w => w.Id);
            modelBuilder.Entity<Word>().HasIndex(w => new { w.Language, w.Text }).IsUnique();
            modelBuilder.Entity<Word>().HasIndex(w => w.Text);

            modelBuilder.Entity<WordRelation>().HasKey(w => w.Id);
            modelBuilder.Entity<WordRelation>().HasIndex(r => new { r.Type, r.FromWordId, r.ToWordId }).IsUnique();
            modelBuilder.Entity<WordRelation>().HasIndex(r => r.FromWordId);
        }
    }
}
