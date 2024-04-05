using AutoMapper;
using BookStore.API.Entities.Author;
using BookStore.API.Entities.Book;
using BookStore.API.Models;

namespace BookStore.API.Configurations;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<AuthorReadViewModel, Author>().ReverseMap();
        CreateMap<AuthorCreateViewModel, Author>().ReverseMap();
        CreateMap<AuthorUpdateViewModel, Author>().ReverseMap();

        CreateMap<Book, BookReadViewModel>()
            .ForMember(x => x
            .AuthorName, y => y
            .MapFrom(z => $"{z.Author.FirstName} {z.Author.LastName}"))
            .ReverseMap();

        CreateMap<Book, BookDetailsViewModel>()
            .ForMember(x => x
            .AuthorName, y => y
            .MapFrom(z => $"{z.Author.FirstName} {z.Author.LastName}"))
            .ReverseMap();

        CreateMap<BookCreateViewModel, Book>().ReverseMap();
        CreateMap<BookUpdateViewModel, Book>().ReverseMap();
    }
}
