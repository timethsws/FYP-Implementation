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
            modelBuilder.Entity<Word>().HasKey(w => w.Id);

            modelBuilder.Entity<WordRelation>().HasKey(wr => wr.Id);
            modelBuilder.Entity<WordRelation>().HasOne(wr => wr.ToWord).WithMany();
            modelBuilder.Entity<WordRelation>().HasOne(wr => wr.FromWord).WithMany(w => w.Relations);

            // TODO : Db Optimaisations
            modelBuilder.Entity<Word>().HasIndex(w => new {w.Language ,w.Text}).IsUnique();

            modelBuilder.Entity<WordRelation>().HasIndex(r => new {r.Type,r.FromWordId,r.ToWordId}).IsUnique();
        }
    }
}
