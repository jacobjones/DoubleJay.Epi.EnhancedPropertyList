using EPiServer.Shell.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace DoubleJay.Epi.EnhancedPropertyList
{
    public static class ServiceCollectionExtensions
    {
        private const string ModuleName = "DoubleJay.Epi.EnhancedPropertyList";
        
        public static IServiceCollection AddEnhancedPropertyList(this IServiceCollection services)
        {
            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i => i.Name.Equals(ModuleName, StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new ModuleDetails {Name = ModuleName});
                    }
                });

            return services;
        }
    }
}