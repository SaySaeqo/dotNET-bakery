using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotNet_bakery.Repo; 
using dotNet_bakery.Models;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System.Text;
using System;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.Extensions.Logging;

namespace dotNet_bakery.Services;

public class Rabbit
{
    private readonly ILogger<Rabbit> _logger;
    private DataRepository _dataRepository;
    private IManagedMqttClient? _mqttClient; 

    public Rabbit(ILogger<Rabbit> logger, DataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
        _logger.LogInformation("Rabbit constructor");
        ConfigureMqttClient();
    }

    private void ConfigureMqttClient(){
        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId("backend")
                .WithTcpServer("rabbitmq", 1883)
                .Build())
            .Build();

        _mqttClient = new MqttFactory().CreateManagedMqttClient();
        _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("sensor/data").Build()).ContinueWith(task => {
            if (task.IsFaulted)
                _logger.LogError($"Failed to subscribe to topic: {task.Exception}");
            else
                _logger.LogInformation("Successfully subscribed to topic");
        });
        _mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            _logger.LogInformation($"Received message: {message}"); 
            DataModel dataModel = JsonConvert.DeserializeObject<DataModel>(message);
            HandleReceivedData(dataModel);
        });
        _mqttClient.StartAsync(options).ContinueWith(task => {
            if (task.IsFaulted)
                _logger.LogError($"Failed to connect to RabbitMQ: {task.Exception}");
            else
                _logger.LogInformation("Successfully connected to RabbitMQ");
        });
    }

    private async void HandleReceivedData(DataModel dataModel){
        await _dataRepository.CreateAsync(dataModel);
    }
}
