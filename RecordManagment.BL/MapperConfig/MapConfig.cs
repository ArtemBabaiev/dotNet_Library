using AutoMapper;
using RecordManagment.BLL.DTO;
using RecordManagment.DAL.Model;
using RecordManagment.DAL.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.MapperConfig
{
    public class MapConfig
    {
        IUnitOfWork unitOfWork;

        public MapConfig(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public MapperConfiguration EmployeeToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDTO>();

            });
        }

        public MapperConfiguration EmployeeFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeDTO, Employee>();

            });
        }

        public MapperConfiguration ExemplarToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Exemplar, ExemplarDTO>()
                .ForMember(
                    d => d.Literature,
                    opt => opt.MapFrom(model =>
                    new Mapper(LiteratureToDTO())
                    .Map<Literature, LiteratureDTO>(unitOfWork.LiteratureRepository.GetAsync(model.LiteratureId).Result)));

            });
        }

        public MapperConfiguration ExemplarFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExemplarDTO, Exemplar>()
                .ForMember(
                    d => d.LiteratureId,
                    opt => opt.MapFrom(dto => dto.Literature.Id)
                    );

            });
        }

        public MapperConfiguration LiteratureToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Literature, LiteratureDTO>();

            });
        }

        public MapperConfiguration LiteratureFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LiteratureDTO, Literature>();

            });
        }

        public MapperConfiguration ReaderToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reader, ReaderDTO>();

            });
        }

        public MapperConfiguration ReaderFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReaderDTO, Reader>();

            });
        }

        public MapperConfiguration RecordToDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Record, RecordDTO>()
                .ForMember(
                    d => d.Exemplar,
                    opt => opt.MapFrom(model => new Mapper(ExemplarToDTO())
                    .Map<Exemplar, ExemplarDTO>(unitOfWork.ExemplarRepository.GetAsync(model.ExemplarId).Result)))
                .ForMember(
                    d => d.Reader,
                    opt => opt.MapFrom(model => new Mapper(ReaderToDTO())
                    .Map<Reader, ReaderDTO>(unitOfWork.ReaderRepository.GetAsync(model.ReaderId).Result)))
                .ForMember(
                    d => d.LendByEmployee,
                    opt => opt.MapFrom(model => new Mapper(EmployeeToDTO())
                    .Map<Employee, EmployeeDTO>(unitOfWork.EmployeeRepository.GetAsync(model.LendByEmployeeId).Result)))
                .ForMember(
                    d => d.AcceptedByEmployee,
                    opt => opt.MapFrom(model => new Mapper(EmployeeToDTO())
                    .Map<Employee, EmployeeDTO>(unitOfWork.EmployeeRepository.GetAsync(model.AcceptedByEmployeeId).Result)));

            });
        }

        public MapperConfiguration RecordFromDTO()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RecordDTO, Record>()
                .ForMember(
                    d => d.ExemplarId,
                    opt => opt.MapFrom(dto => dto.Exemplar.Id))
                .ForMember(
                    d => d.ReaderId,
                    opt => opt.MapFrom(dto => dto.Reader.Id))
                .ForMember(
                    d => d.LendByEmployeeId,
                    opt => opt.MapFrom(dto => dto.LendByEmployee.Id))
                .ForMember(
                    d => d.AcceptedByEmployeeId,
                    opt => opt.MapFrom(dto => dto.AcceptedByEmployee.Id));

            });
        }
    }
}
