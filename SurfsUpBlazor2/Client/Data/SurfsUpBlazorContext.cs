using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUpBlazor.Model;
using System.Collections.Generic;

namespace SurfsUpBlazor.Data
{
    public class SurfsUpBlazorContext : IdentityDbContext
    {

        public SurfsUpBlazorContext(DbContextOptions<SurfsUpBlazorContext> options)
            : base(options)
        {
        }
        public DbSet<Board> Board { get; set; } = default!;

        public DbSet<Rent> Rent { get; set; }

    }
}
