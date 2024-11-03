
using HRMSApplication.Core.Interfaces;
using HRMSApplication.Core.Models;

namespace HRMSApplication.Infrastructure
{
    public class EmployeeBizLayer : IEmployeeBizLayer
    { 
        private readonly IUnitOfWork _unitOfWork; 
        public EmployeeBizLayer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddEmployeeAsync(Employee Employee)
        { 
            await _unitOfWork.Employee.AddAsync(Employee);
            await _unitOfWork.CompleteAsync(); 
        }

        public async Task<IEnumerable<Employee>> GetEmployeeAsync()
        { 
            var result = await _unitOfWork.Employee.GetAllAsync(); 
             
            return result;
        } 

        public async Task<Employee> GetEmployeeAsync(int id)
        { 
            var result = await _unitOfWork.Employee.GetByIdAsync(id); 
          
            return result;
        }

        public async Task ModifyEmployeeAsync(Employee person)
        {
            await _unitOfWork.Employee.UpdateAsync(person);
            await _unitOfWork.CompleteAsync();
            
            //Some business logic
        }

        public async Task RemoveEmployeeAsync(int id)
        {  
            await _unitOfWork.Employee.RemoveAsync(id);
            
            await _unitOfWork.CompleteAsync();  
        }
    }
}
