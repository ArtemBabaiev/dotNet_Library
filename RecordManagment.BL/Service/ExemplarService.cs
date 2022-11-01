using AutoMapper;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.MapperConfig;
using RecordManagment.BLL.Service.Interface;
using RecordManagment.DAL.Model;
using RecordManagment.DAL.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service
{
    public class ExemplarService: IExemplarService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapConfig mapperConfig;

        public ExemplarService(IUnitOfWork unitOfWork, MapConfig mapperConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapperConfig = mapperConfig;
        }

        public async Task<ExemplarDTO> CreateExemplar(ExemplarDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.ExemplarToDTO();
            MapperConfiguration configFromDTO = mapperConfig.ExemplarFromDTO();
            Exemplar employee = new Mapper(configFromDTO).Map<Exemplar>(employeeDTO);
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            long newId = await unitOfWork.ExemplarRepository.AddAsync(employee);
            Exemplar newExemplar = await unitOfWork.ExemplarRepository.GetAsync(newId);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<ExemplarDTO>(newExemplar);

        }

        public async Task DeleteExemplar(long id)
        {
            await unitOfWork.ExemplarRepository.DeleteAsync(id);
            unitOfWork.Commit();
        }

        public async Task<List<ExemplarDTO>> GetAllExemplars()
        {
            MapperConfiguration configToDTO = mapperConfig.ExemplarToDTO();
            var mapper = new Mapper(configToDTO);
            var result = mapper.Map<IEnumerable<Exemplar>, List<ExemplarDTO>>(await unitOfWork.ExemplarRepository.GetAllAsync());
            unitOfWork.Commit();
            return result;
        }

        public async Task<ExemplarDTO> GetExemplarById(long id)
        {
            MapperConfiguration config = mapperConfig.ExemplarToDTO();
            var employee = await unitOfWork.ExemplarRepository.GetAsync(id);
            unitOfWork.Commit();
            var mapper = new Mapper(config);
            var result = mapper.Map<ExemplarDTO>(employee);
            return result;
        }

        public async Task<ExemplarDTO> UpdateExemplar(ExemplarDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.ExemplarToDTO();
            MapperConfiguration configFromDTO = mapperConfig.ExemplarFromDTO();

            var toUpdate = new Mapper(configFromDTO).Map<Exemplar>(employeeDTO);
            toUpdate.UpdatedAt = DateTime.Now;
            await unitOfWork.ExemplarRepository.ReplaceAsync(toUpdate);
            Exemplar updatedExemplar = await unitOfWork.ExemplarRepository.GetAsync(toUpdate.Id);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<ExemplarDTO>(updatedExemplar);
        }
    }
}
