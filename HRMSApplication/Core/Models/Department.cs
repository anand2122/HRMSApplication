using Newtonsoft.Json;

namespace HRMSApplication.Core.Models
{
    public class Department
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int DepartmentNo { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
