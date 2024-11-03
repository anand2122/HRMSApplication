using HRMSApplication.Core.Models;
using HRMSApplication.Infrastructure.Repositories;
using System;

namespace HRMSApplication.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {  
        IRepository<Employee> Employee { get; }
        IDepartmentRepository DepartmentRepository { get; }
        Task<int> CompleteAsync();
    }
}
