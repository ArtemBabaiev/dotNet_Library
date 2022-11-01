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
    public class LiteratureService: ILiteratureService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapConfig mapperConfig;

        public LiteratureService(IUnitOfWork unitOfWork, MapConfig mapperConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapperConfig = mapperConfig;
        }

        public async Task<LiteratureDTO> CreateLiterature(LiteratureDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.LiteratureToDTO();
            MapperConfiguration configFromDTO = mapperConfig.LiteratureFromDTO();
            Literature employee = new Mapper(configFromDTO).Map<Literature>(employeeDTO);
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            long newId = await unitOfWork.LiteratureRepository.AddAsync(employee);
            Literature newLiterature = await unitOfWork.LiteratureRepository.GetAsync(newId);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<LiteratureDTO>(newLiterature);

        }

        public async Task DeleteLiterature(long id)
        {
            await unitOfWork.LiteratureRepository.DeleteAsync(id);
            unitOfWork.Commit();
        }

        public async Task<List<LiteratureDTO>> GetAllLiterature()
        {
            MapperConfiguration configToDTO = mapperConfig.LiteratureToDTO();
            var mapper = new Mapper(configToDTO);
            var result = mapper.Map<IEnumerable<Literature>, List<LiteratureDTO>>(await unitOfWork.LiteratureRepository.GetAllAsync());
            unitOfWork.Commit();
            return result;
        }

        public async Task<LiteratureDTO> GetLiteratureById(long id)
        {
            MapperConfiguration config = mapperConfig.LiteratureToDTO();
            var employee = await unitOfWork.LiteratureRepository.GetAsync(id);
            unitOfWork.Commit();
            var mapper = new Mapper(config);
            var result = mapper.Map<LiteratureDTO>(employee);
            return result;
        }

        public async Task<LiteratureDTO> UpdateLiterature(LiteratureDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.LiteratureToDTO();
            MapperConfiguration configFromDTO = mapperConfig.LiteratureFromDTO();

            var toUpdate = new Mapper(configFromDTO).Map<Literature>(employeeDTO);
            toUpdate.UpdatedAt = DateTime.Now;
            await unitOfWork.LiteratureRepository.ReplaceAsync(toUpdate);
            Literature updatedLiterature = await unitOfWork.LiteratureRepository.GetAsync(toUpdate.Id);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<LiteratureDTO>(updatedLiterature);
        }
    }
}
