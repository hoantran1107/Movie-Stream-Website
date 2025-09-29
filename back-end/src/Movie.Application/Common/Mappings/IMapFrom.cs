using AutoMapper;

namespace Movie.Application.Common.Mappings;

/// <summary>
/// Interface for objects that can be mapped from a source type
/// </summary>
/// <typeparam name="T">The source type</typeparam>
public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
