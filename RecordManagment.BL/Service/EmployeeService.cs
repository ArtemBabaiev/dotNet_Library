using AutoMapper;
using Dapper;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.MapperConfig;
using RecordManagment.BLL.Service.Interface;
using RecordManagment.DAL.Model;
using RecordManagment.DAL.UnitOfWork;
using RecordManagment.DAL.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MapConfig mapperConfig;

        public EmployeeService(IUnitOfWork unitOfWork, MapConfig mapperConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapperConfig = mapperConfig;
        }

        public async Task<EmployeeDTO> CreateEmployee(EmployeeDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.EmployeeToDTO();
            MapperConfiguration configFromDTO = mapperConfig.EmployeeFromDTO();
            Employee employee = new Mapper(configFromDTO).Map<Employee>(employeeDTO);
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            long newId = await unitOfWork.EmployeeRepository.AddAsync(employee);
            Employee newEmployee = await unitOfWork.EmployeeRepository.GetAsync(newId);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<EmployeeDTO>(newEmployee);

        }

        public async Task DeleteEmployee(long id)
        {
            await unitOfWork.EmployeeRepository.DeleteAsync(id);
            unitOfWork.Commit();
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            MapperConfiguration configToDTO = mapperConfig.EmployeeToDTO();
            var mapper = new Mapper(configToDTO);
            var result = mapper.Map<IEnumerable<Employee>, List<EmployeeDTO>>(await unitOfWork.EmployeeRepository.GetAllAsync());
            unitOfWork.Commit();
            return result;
        }

        public async Task<EmployeeDTO> GetEmployeeById(long id)
        {
            MapperConfiguration config = mapperConfig.EmployeeToDTO();
            var employee = await unitOfWork.EmployeeRepository.GetAsync(id);
            unitOfWork.Commit();
            var mapper = new Mapper(config);
            var result = mapper.Map<EmployeeDTO>(employee);
            return result;
        }

        public async Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO)
        {
            MapperConfiguration configToDTO = mapperConfig.EmployeeToDTO();
            MapperConfiguration configFromDTO = mapperConfig.EmployeeFromDTO();

            var toUpdate = new Mapper(configFromDTO).Map<Employee>(employeeDTO);
            toUpdate.UpdatedAt = DateTime.Now;
            await unitOfWork.EmployeeRepository.ReplaceAsync(toUpdate);
            Employee updatedEmployee = await unitOfWork.EmployeeRepository.GetAsync(toUpdate.Id);
            unitOfWork.Commit();
            return new Mapper(configToDTO).Map<EmployeeDTO>(updatedEmployee); 
        }
    }
}
