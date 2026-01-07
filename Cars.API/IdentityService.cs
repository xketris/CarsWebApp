using Cars.Domain;
using Cars.Infrastructure;

namespace Cars.API
{
    public static class IdentityService
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<DataContext>();
            services.AddAuthentication();
            return services;
        }
    }
}
