using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Data
{
    public class EuroBankContext:DbContext
    {
        public EuroBankContext() { }
        public EuroBankContext(DbContextOptions<EuroBankContext> options) : base(options) { }

        //DbSet Tables For the Context

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
