using AtosCustomer.Api.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace AtosCustomer.Tests
{
    public class TestApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                //Remove existing repository registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICustomerRepository));

                if (descriptor != null)
                    services.Remove(descriptor);

                //Add fresh repository for tests
                services.AddSingleton<ICustomerRepository, CustomerRepository>();
            });
        }
    }
}
