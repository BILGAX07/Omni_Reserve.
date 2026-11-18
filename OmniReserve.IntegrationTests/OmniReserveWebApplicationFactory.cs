
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniReserve.Infrastructure.Persistence;

namespace OmniReserve.IntegrationTests;

public class OmniReserveWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                descriptor => descriptor.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextOptionsDescriptor is not null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            var dbContextOptionsConfigurationDescriptors = services
                .Where(descriptor =>
                    descriptor.ServiceType.Name.Contains(
                        "IDbContextOptionsConfiguration"))
                .ToList();

            foreach (var descriptor in dbContextOptionsConfigurationDescriptors)
            {
                services.Remove(descriptor);
            }

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.RemoveAll<IDistributedCache>();
            services.AddDistributedMemoryCache();
        });
    }
}
