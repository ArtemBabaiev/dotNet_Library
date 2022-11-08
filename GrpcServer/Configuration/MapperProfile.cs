using AutoMapper;
using Catalog.DAL.Entity;
using GrpcServer.Protos;
using LiteratureEntity = Catalog.DAL.Entity.Literature;

namespace GrpcServer.Configuration
{
    public class MapperProfile : Profile
    {
        protected MapperProfile()
        {
            CreateProtoModelMap();
        }

        public void CreateProtoModelMap()
        {
            CreateMap<LiteratureEntity, LiteratureModel>();
        }
    }
}
