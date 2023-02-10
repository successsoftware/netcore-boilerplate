using AutoMapper;

namespace CoreApiTemplate.Application.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
