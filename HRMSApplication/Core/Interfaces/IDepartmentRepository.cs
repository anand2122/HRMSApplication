using HRMSApplication.Core.Models;

namespace HRMSApplication.Core.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(string id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(string id);
    }
}
