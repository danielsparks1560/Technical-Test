using Property_Viewing_Slots_API.Services;
using Property_Viewing_Slots_API.Storage;

namespace Property_Viewing_Slots_API.Extensions
{
    public static class PropertyViewingExtensions
    {
        public static IServiceCollection AddPropertyViewingServices(this IServiceCollection services)
        {
            services.AddSingleton<IPropertyViewingStorage, PropertyViewingStorage>();
            services.AddScoped<IPropertyViewingService, PropertyViewingService>();
            return services;
        }
    }
}
