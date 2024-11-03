using HRMSApplication.Core.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace HRMSApplication.Infrastructure.Data
{
    public class DepartmentContext  
    {
        public Container Container { get; private set; }

        public DepartmentContext(IConfiguration configuration)
        {
            var cosmosClient = new CosmosClient(
                configuration["CosmosDb:AccountEndpoint"],
                configuration["CosmosDb:AccountKey"]
            );

            var database = cosmosClient.GetDatabase(configuration["CosmosDb:DatabaseId"]);
            Container = database.GetContainer(configuration["CosmosDb:ContainerId"]);
        }
    }
}