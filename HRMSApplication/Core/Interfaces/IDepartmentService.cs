namespace HRMSApplication.Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<bool> DepartmentExistsAsync(int departmentNo);
    }
}
