using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Models;

namespace MvcMusicDistr.Data
{
    public class MvcMusicDistrContext : DbContext
    {
        public MvcMusicDistrContext (DbContextOptions<MvcMusicDistrContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMusicDistr.Models.Artist> Artist { get; set; } = default!;
        public DbSet<MvcMusicDistr.Models.Beat> Beat { get; set; } = default!;
        public DbSet<MvcMusicDistr.Models.Gait> Gait { get; set; } = default!;
        public DbSet<MvcMusicDistr.Models.Pair> Pair { get; set; } = default!;
        public DbSet<MvcMusicDistr.Models.Distribution> Distribution { get; set; } = default!;
        
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Artist>()
        //        .HasIndex(u => u.Nickname)
        //        .IsUnique();
                     

        //}
    }
}
