using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Model;

namespace SurfsUpAPI
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
           : base(options)
        {
        }
        public DbSet<Board> Board { get; set; } = default!;

        public DbSet<Rent> Rent { get; set; }
    }
}
