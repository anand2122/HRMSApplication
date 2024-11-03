using HRMSApplication.Core.Interfaces;
using HRMSApplication.Core.Models;
using HRMSApplication.Infrastructure.Data;
using Microsoft.Azure.Cosmos;
using HRMSApplication.Core.Models;

namespace HRMSApplication.Infrastructure.Repositories
{
    public class DepartmentRepository :IDepartmentRepository
    {
        private readonly Container _container;

        public DepartmentRepository(DepartmentContext cosmosDbContext)
        {
            _container = cosmosDbContext.Container;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Department>("SELECT * FROM c");
            var results = new List<Department>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Department> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Department>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException)
            {
                return null; // Handle not found exception, log if necessary
            }
        }

        public async Task AddAsync(Department department)
        {
            await _container.CreateItemAsync(department, new PartitionKey(department.Id));
        }

        public async Task UpdateAsync(Department department)
        {
            await _container.ReplaceItemAsync(department, department.Id, new PartitionKey(department.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Department>(id, new PartitionKey(id));
        }
    }
}
