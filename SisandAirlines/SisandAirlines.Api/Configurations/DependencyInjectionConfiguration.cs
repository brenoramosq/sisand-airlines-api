using SisandAirlines.Domain.Interfaces.Auth;
using SisandAirlines.Domain.Interfaces.Externals;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using SisandAirlines.Infra.Auth;
using SisandAirlines.Infra.DAO;
using SisandAirlines.Infra.DAO.Interfaces;
using SisandAirlines.Infra.Externals;
using SisandAirlines.Infra.Repositories;
using SisandAirlines.Infra.UoW;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;

namespace SisandAirlines.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionExtension(this IServiceCollection services, IConfiguration configuration)
        {
            // repositories                                               
            services.AddScoped<IUnitOfWork, UnitOfWork>(provider =>
                new UnitOfWork(configuration["ConnectionStrings:Postgres"])
            );

            services.AddScoped<IFlightRepository, FlightRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                
                return new FlightRepository(connectionString, unitOfWork);
            });

            services.AddScoped<IAirplaneRepository, AirplaneRepository>(provider => new AirplaneRepository
            (
               connectionString: configuration["ConnectionStrings:Postgres"]
            ));

            services.AddScoped<ICustomerRepository, CustomerRepository>(provider => new CustomerRepository
            (
                connectionString: configuration["connectionstrings:postgres"]
            ));

            
            services.AddScoped<ISeatRepository, SeatRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                return new SeatRepository(connectionString, unitOfWork);
            });

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                return new ShoppingCartRepository(connectionString, unitOfWork);
            });

            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                return new ShoppingCartItemRepository(connectionString, unitOfWork);
            });

            services.AddScoped<IPurchaseRepository, PurchaseRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                return new PurchaseRepository(connectionString, unitOfWork);
            });

            services.AddScoped<ITicketRepository, TicketRepository>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var connectionString = configuration["ConnectionStrings:Postgres"];
                return new TicketRepository(connectionString, unitOfWork);
            });


            // dao
            services.AddScoped<IFlightDAO, FlightDAO>(provider => new FlightDAO
            (
                connectionString: configuration["ConnectionStrings:Postgres"]
            ));

            // others
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService>(provider => new JwtService(configuration["Jwt:SecretKey"], configuration["Jwt:Issuer"], configuration["Jwt:Audience"]));
        }
    }
}
