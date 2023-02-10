using AutoMapper;

namespace CoreApiTemplate.Application.Mapping
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile);
    }
}
