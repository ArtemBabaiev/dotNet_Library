using AutoMapper;
using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.DTO.Response;
using WrittenOffManagement.Domain.Entities;

namespace WrittenOffManagement.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateWrittenOffMap();
            CreateEmployeeMap();
            CreateWrritenOffEventMap();
        }

        public void CreateEmployeeMap()
        {
            CreateMap<EmployeeRequest, Employee>();
            CreateMap<Employee, EmployeeResponse>();
        }

        public void CreateWrittenOffMap()
        {
            CreateMap<WrittenOffRequest, WrittenOff>();
            CreateMap<WrittenOff, WrittenOffResponse>();
        }

        public void CreateWrritenOffEventMap()
        {
            CreateMap<WriteOffExemplarEvent, WrittenOff>()
                .ForMember(entity => entity.Id,
                conf => conf.MapFrom(
                    wEvent => wEvent.LiteratureId));
        }

    }
}
