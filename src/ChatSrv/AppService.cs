using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Server;
using OpenMir2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGate
{
    public class AppService : IHostedService
    {

        private readonly MqttServer _mqttServer;

        public AppService()
        {
            MqttFactory mqttFactory = new MqttFactory(new QueueConsoleLogger());
            MqttServerOptions mqttServerOptions = mqttFactory.CreateServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(7883)
                .Build();
            _mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogService.Info("ChatService is starting.");
            _mqttServer.StartedAsync += e =>
            {
                Console.WriteLine("MQTT server started.");
                return Task.CompletedTask;
            };
            _mqttServer.ClientConnectedAsync += e =>
            {
                Console.WriteLine($"Client connected: {e.ClientId}");
                return Task.CompletedTask;
            };
            _mqttServer.ClientDisconnectedAsync += e =>
            {
                Console.WriteLine($"Client disconnected: {e.ClientId}");
                return Task.CompletedTask;
            };
            _mqttServer.ClientSubscribedTopicAsync += e =>
            {
                Console.WriteLine($"Client subscribed: {e.ClientId}, {e.TopicFilter}");
                return Task.CompletedTask;
            };
            _mqttServer.ClientUnsubscribedTopicAsync += e =>
            {
                Console.WriteLine($"Client unsubscribed: {e.ClientId}, {e.TopicFilter}");
                return Task.CompletedTask;
            };
            await _mqttServer.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            LogService.Info("ChatService is stopping.");
            return _mqttServer.StopAsync();
        }
    }
}