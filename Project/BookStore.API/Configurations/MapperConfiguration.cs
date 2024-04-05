using AutoMapper;
using BookStore.API.Entities.Author;
using BookStore.API.Models;

namespace BookStore.API.Configurations;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<AuthorReadViewModel, Author>().ReverseMap();
        CreateMap<AuthorCreateViewModel, Author>().ReverseMap();
        CreateMap<AuthorUpdateViewModel, Author>().ReverseMap();
    }
}
