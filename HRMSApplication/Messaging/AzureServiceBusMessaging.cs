using Microsoft.Azure.ServiceBus;
using System.Text;

namespace HRMSApplication.Messaging
{
    public class AzureServiceBusMessaging
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private QueueClient _queueClient;

        public AzureServiceBusMessaging(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
            _queueClient = new QueueClient(_connectionString, _queueName);
        }

        public async Task SendMessageAsync(string message)
        {
            var busMessage = new Message(Encoding.UTF8.GetBytes(message));
            await _queueClient.SendAsync(busMessage);
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            _queueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    // Process the message
                    var receivedMessage = Encoding.UTF8.GetString(message.Body);
                    Console.WriteLine($"Received message: {receivedMessage}");
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                },
                new MessageHandlerOptions(ExceptionHandler) { MaxConcurrentCalls = 1, AutoComplete = false });
        }

        private Task ExceptionHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            return Task.CompletedTask;
        }
    }
}
