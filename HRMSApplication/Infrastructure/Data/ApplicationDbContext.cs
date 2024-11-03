using HRMSApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMSApplication.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        } 
        public DbSet<HRMSApplication.Core.Models.Department> Department { get; set; } = default!;
    }
}
