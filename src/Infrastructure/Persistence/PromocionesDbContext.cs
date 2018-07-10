
using Microsoft.EntityFrameworkCore;
using Promociones.Domain.Entities;

namespace Promociones.Infrastructure.Persistence
{
    public class PromocionesDbContext:DbContext
    {
        public PromocionesDbContext(DbContextOptions<PromocionesDbContext> options)
           : base(options)
        {
        }

        public DbSet<Promocion> Promociones { get; set; }

    }
}
