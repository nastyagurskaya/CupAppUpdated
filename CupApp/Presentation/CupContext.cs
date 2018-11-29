using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CupApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CupApp.Presentation
{
    public class CupContext : DbContext
    {
        
        public DbSet<Cup> Cups { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CupImage> CupImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //modelBuilder.Entity<Cup>().HasOne(c => c.CupImage).WithOne(ci => ci.Cup).OnDelete(DeleteBehavior.Cascade).HasForeignKey<CupImage>(b => b.CupImageID); ;
        }

        public CupContext(DbContextOptions<CupContext> options) : base(options)
        {
            
        }

        public CupContext()
        {
        }
    }
}