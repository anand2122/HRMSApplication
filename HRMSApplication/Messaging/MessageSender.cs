using System.Text.Json;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace HRMSApplication.Messaging
{
    public class MessageSender
    {
        private readonly IConfiguration _configuration; 
        private readonly ServiceBusClient _client; 
        private readonly ServiceBusSender _sender; 
        public MessageSender(IConfiguration configuration) 
        { 
            _configuration = configuration; 
            _client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]); 
            _sender = _client.CreateSender(_configuration["QueueName"]); 
        }
        public async Task SendMessageAsync(string messageBody) 
        { 
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody)); await _sender.SendMessageAsync(message);
        }
    }
}
