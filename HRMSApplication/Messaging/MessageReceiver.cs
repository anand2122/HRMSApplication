using System.Text.Json;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace HRMSApplication.Messaging
{
    public class MessageReceiver
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client; 
        private readonly ServiceBusProcessor _processor; 
        public MessageReceiver(IConfiguration configuration)
        { _configuration = configuration; 
            _client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]);
            _processor = _client.CreateProcessor(_configuration["QueueName"], 
                new ServiceBusProcessorOptions()); 
        }
        public void RegisterMessageHandler() 
        { 
            _processor.ProcessMessageAsync += MessageHandler; 
            _processor.ProcessErrorAsync += ErrorHandler; 
            _processor.StartProcessingAsync(); 
        }
        private async Task MessageHandler(ProcessMessageEventArgs args)
        { 
            string body = Encoding.UTF8.GetString(args.Message.Body);
            Console.WriteLine($"Received message: {body}"); 
            await args.CompleteMessageAsync(args.Message); 
        }
        private Task ErrorHandler(ProcessErrorEventArgs args) 
        { 
            Console.WriteLine($"Message handler encountered an exception {args.Exception}."); 
            return Task.CompletedTask; 
        }
    }
}
