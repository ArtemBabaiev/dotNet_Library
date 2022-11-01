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
    public class RecordService: IRecordService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapConfig mapperConfig;

        public RecordService(IUnitOfWork unitOfWork, MapConfig mapperConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapperConfig = mapperConfig;
        }

        public async Task<RecordDTO> CreateRecord(RecordDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.RecordToDTO();
            MapperConfiguration configFromDTO = mapperConfig.RecordFromDTO();
            Record employee = new Mapper(configFromDTO).Map<Record>(employeeDTO);
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            long newId = await unitOfWork.RecordRepository.AddAsync(employee);
            Record newRecord = await unitOfWork.RecordRepository.GetAsync(newId);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<RecordDTO>(newRecord);

        }

        public async Task DeleteRecord(long id)
        {
            await unitOfWork.RecordRepository.DeleteAsync(id);
            unitOfWork.Commit();
        }

        public async Task<List<RecordDTO>> GetAllRecords()
        {
            MapperConfiguration configToDTO = mapperConfig.RecordToDTO();
            var mapper = new Mapper(configToDTO);
            var result = mapper.Map<IEnumerable<Record>, List<RecordDTO>>(await unitOfWork.RecordRepository.GetAllAsync());
            unitOfWork.Commit();
            return result;
        }

        public async Task<RecordDTO> GetRecordById(long id)
        {
            MapperConfiguration config = mapperConfig.RecordToDTO();
            var employee = await unitOfWork.RecordRepository.GetAsync(id);
            unitOfWork.Commit();
            var mapper = new Mapper(config);
            var result = mapper.Map<RecordDTO>(employee);
            return result;
        }

        public async Task<RecordDTO> UpdateRecord(RecordDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.RecordToDTO();
            MapperConfiguration configFromDTO = mapperConfig.RecordFromDTO();

            var toUpdate = new Mapper(configFromDTO).Map<Record>(employeeDTO);
            toUpdate.UpdatedAt = DateTime.Now;
            await unitOfWork.RecordRepository.ReplaceAsync(toUpdate);
            Record updatedRecord = await unitOfWork.RecordRepository.GetAsync(toUpdate.Id);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<RecordDTO>(updatedRecord);
        }
    }
}
