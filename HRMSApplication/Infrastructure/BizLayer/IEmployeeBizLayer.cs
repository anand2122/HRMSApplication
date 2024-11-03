 
using HRMSApplication.Core.Models;
 
    public interface IEmployeeBizLayer
    {
        Task<IEnumerable<Employee>> GetEmployeeAsync(); 

        Task<Employee> GetEmployeeAsync(int id);

        Task AddEmployeeAsync(Employee employee);

        Task ModifyEmployeeAsync(Employee employee);

        Task RemoveEmployeeAsync(int id);
    }
 
