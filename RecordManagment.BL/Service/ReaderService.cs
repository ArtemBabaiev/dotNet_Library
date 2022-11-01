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
    public class ReaderService: IReaderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapConfig mapperConfig;

        public ReaderService(IUnitOfWork unitOfWork, MapConfig mapperConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapperConfig = mapperConfig;
        }

        public async Task<ReaderDTO> CreateReader(ReaderDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.ReaderToDTO();
            MapperConfiguration configFromDTO = mapperConfig.ReaderFromDTO();
            Reader employee = new Mapper(configFromDTO).Map<Reader>(employeeDTO);
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            long newId = await unitOfWork.ReaderRepository.AddAsync(employee);
            Reader newReader = await unitOfWork.ReaderRepository.GetAsync(newId);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<ReaderDTO>(newReader);

        }

        public async Task DeleteReader(long id)
        {
            await unitOfWork.ReaderRepository.DeleteAsync(id);
            unitOfWork.Commit();
        }

        public async Task<List<ReaderDTO>> GetAllReaders()
        {
            MapperConfiguration configToDTO = mapperConfig.ReaderToDTO();
            var mapper = new Mapper(configToDTO);
            var result = mapper.Map<IEnumerable<Reader>, List<ReaderDTO>>(await unitOfWork.ReaderRepository.GetAllAsync());
            unitOfWork.Commit();
            return result;
        }

        public async Task<ReaderDTO> GetReaderById(long id)
        {
            MapperConfiguration config = mapperConfig.ReaderToDTO();
            var employee = await unitOfWork.ReaderRepository.GetAsync(id);
            unitOfWork.Commit();
            var mapper = new Mapper(config);
            var result = mapper.Map<ReaderDTO>(employee);
            return result;
        }

        public async Task<ReaderDTO> UpdateReader(ReaderDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.ReaderToDTO();
            MapperConfiguration configFromDTO = mapperConfig.ReaderFromDTO();

            var toUpdate = new Mapper(configFromDTO).Map<Reader>(employeeDTO);
            toUpdate.UpdatedAt = DateTime.Now;
            await unitOfWork.ReaderRepository.ReplaceAsync(toUpdate);
            Reader updatedReader = await unitOfWork.ReaderRepository.GetAsync(toUpdate.Id);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<ReaderDTO>(updatedReader);
        }
    }
}
