using AutoMapper;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.DAL.Entity;
using Type = Catalog.DAL.Entity.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateAuthorMap();
            CreateExemplarMap();
            CreateGenreMap();
            CreateLiteratureMap();
            CreatePublisherMap();
            CreateTypeMap();
            CreateWritingMap();
        }

        public void CreateAuthorMap()
        {
            CreateMap<AuthorRequest, Author>();
            CreateMap<Author, AuthorResponse>();
        }

        public void CreateExemplarMap()
        {
            CreateMap<ExemplarRequest, Exemplar>();
            CreateMap<Exemplar, ExemplarResponse>();
        }

        public void CreateGenreMap()
        {
            CreateMap<GenreRequest, Genre>();
            CreateMap<Genre, GenreResponse>();
        }

        public void CreateLiteratureMap()
        {
            CreateMap<LiteratureRequest, Literature>()
                ;
            CreateMap<Literature, LiteratureResponse>()
                /*.ForMember(
                    response => response.Author, 
                    conf => conf.MapFrom(
                        model => model.Author))
                .ForMember(
                    response => response.Publisher,
                    conf => conf.MapFrom(
                        model => model.Publisher))
                .ForMember(
                    response => response.Genre,
                    conf => conf.MapFrom(
                        model => model.Genre))
                .ForMember(
                    response => response.Type,
                    conf => conf.MapFrom(
                        model => model.Type))*/
                ;
        }
        public void CreatePublisherMap()
        {
            CreateMap<PublisherRequest, Publisher>();
            CreateMap<Publisher, PublisherResponse>();
        }

        public void CreateTypeMap()
        {
            CreateMap<TypeRequest, Type>();
            CreateMap<Type, TypeResponse>();
        }

        public void CreateWritingMap()
        {
            CreateMap<WritingRequest, Writing>();
            CreateMap<Writing, WritingResponse>();
        }


    }
}
