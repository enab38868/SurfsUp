using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt.Models;

namespace SurfsUpProjekt.Data
{
    public class SurfsUpProjektContext : DbContext
    {
        public SurfsUpProjektContext (DbContextOptions<SurfsUpProjektContext> options)
            : base(options)
        {
        }

        public DbSet<SurfsUpProjekt.Models.Board> Board { get; set; } = default!;
    }
}
