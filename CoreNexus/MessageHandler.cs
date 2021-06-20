using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreNexus
{
    public class MessageHandler
    {
         DatasetObject consumedMessage;

        public MessageHandler()
        {

        }

        public void MessageRouting()
        {
            RabbitmqClient client = new RabbitmqClient();
            using var connection = client.factory.CreateConnection();
            using var uiChannel = connection.CreateModel();
            using var advChannel = connection.CreateModel();

            uiChannel.QueueDeclare("UI-output-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            advChannel.QueueDeclare("AdviesTool-input-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            var consumer = new EventingBasicConsumer(uiChannel);
            consumer.Received += (sender, e) =>
            {
                advChannel.BasicPublish("", "AdviesTool-input-queue", null, e.Body);

            };
            
        }

    }


}
