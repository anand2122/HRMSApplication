using HRMSApplication.Core.Interfaces;
using HRMSApplication.Core.Models;
using HRMSApplication.Infrastructure.Repositories;
using MVC_DemoEFCore.Repositories;
 

namespace HRMSApplication.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly DepartmentContext _deptContext;

        public IRepository<Employee> Employees { get; }


        public IDepartmentRepository DepartmentRepository { get; }
        public IRepository<Employee> Employee { get; }

        public UnitOfWork(ApplicationDbContext employeeDbContext , DepartmentContext deptContext, IDepartmentRepository departmentRepository)
        {
            _context = employeeDbContext;
            _deptContext = deptContext;
            DepartmentRepository = departmentRepository;
            Employee = new Repository<Employee>(_context);
        } 
  

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.DisposeAsync().AsTask().Wait();
        }
    }
}
