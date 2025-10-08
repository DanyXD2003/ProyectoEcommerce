using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
    {
        public EcommerceDbContext CreateDbContext(string[] args)
        {
            var cs = "Host=ep-wispy-violet-a8e8t1sz-pooler.eastus2.azure.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_2hgIfCKpy5Ea;SSL Mode=Require;Trust Server Certificate=true";
            var opts = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseNpgsql(cs)
                .Options;
            return new EcommerceDbContext(opts);
        }
    }
}
