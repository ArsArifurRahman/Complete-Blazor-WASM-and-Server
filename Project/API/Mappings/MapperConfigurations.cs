using API.Entities.Models;
using API.Entities.ViewModels.Author;
using API.Entities.ViewModels.Book;
using AutoMapper;

namespace API.Mappings;

public class MapperConfigurations : Profile
{
    public MapperConfigurations()
    {
        CreateMap<Author, AuthorRead>().ReverseMap();
        CreateMap<Author, AuthorCreate>().ReverseMap();
        CreateMap<Author, AuthorUpdate>().ReverseMap();

        CreateMap<Book, BookCreate>().ReverseMap();
        CreateMap<Book, BookUpdate>().ReverseMap();
        CreateMap<Book, BookRead>()
            .ForMember(q => q.AuthorName, d => d
            .MapFrom(map => map.Author != null ? $"{map.Author.FirstName} {map.Author.LastName}" : ""))
            .ForMember(q => q.AuthorId, opt => opt.MapFrom(src => src.Author))
            .ReverseMap();

        CreateMap<Book, BookDetails>()
            .ForMember(q => q.AuthorName, d => d
            .MapFrom(map => map.Author != null ? $"{map.Author.FirstName} {map.Author.LastName}" : ""))
            .ForMember(q => q.AuthorId, opt => opt.MapFrom(src => src.Author))
            .ReverseMap();
    }
}
