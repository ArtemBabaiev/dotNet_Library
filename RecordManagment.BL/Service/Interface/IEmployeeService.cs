using RecordManagment.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> CreateEmployee(EmployeeDTO employeeDTO);
        Task<EmployeeDTO> GetEmployeeById(long id);
        Task<List<EmployeeDTO>> GetAllEmployees();
        Task DeleteEmployee(long id);
        Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO);
    }
}
