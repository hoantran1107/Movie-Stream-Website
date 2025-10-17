using AutoMapper;

namespace Movie.Application.Common.Mappings;

/// <summary>
/// Base mapping profile for AutoMapper configurations
/// </summary>
public abstract class MappingProfile : Profile
{
    protected MappingProfile()
    {
        ApplyMappingsFromAssembly();
    }

    private void ApplyMappingsFromAssembly()
    {
        var assembly = typeof(MappingProfile).Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping") ??
                           type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
