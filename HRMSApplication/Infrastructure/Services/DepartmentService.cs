using HRMSApplication.Core.Interfaces;
using HRMSApplication.Core.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;

namespace HRMSApplication.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly Container _container;
        public DepartmentService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task<bool> DepartmentExistsAsync(int departmentNo)
        {
            var query = $"SELECT * FROM c WHERE c.DepartmentNo = {departmentNo}";
            var iterator = _container.GetItemQueryIterator<Department>(new QueryDefinition(query));
                while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                if (response.Count > 0)
                { return true;
                } }
            return false; 
        }
    }
}

