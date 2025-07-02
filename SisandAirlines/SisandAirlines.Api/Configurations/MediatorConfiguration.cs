using SisandAirlines.Application;

namespace SisandAirlines.Api.Configurations
{
    public static class MediatorConfiguration
    {
        public static void AddMediatorExtension(this IServiceCollection services)
        {
           services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly)
           );
        }
    }
}
