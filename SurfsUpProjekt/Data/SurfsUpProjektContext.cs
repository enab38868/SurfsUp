using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt.Models;

namespace SurfsUpProjekt.Data
{
    public class SurfsUpProjektContext : IdentityDbContext
    {
        public SurfsUpProjektContext (DbContextOptions<SurfsUpProjektContext> options)
            : base(options)
        {
        }
        public DbSet<Board> Board { get; set; } = default!;

        public DbSet<Rent> Rent { get; set; }
    }
}   
