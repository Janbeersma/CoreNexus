using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreNexus
{
    class RabbitmqClient
    {
        public ConnectionFactory factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };        

        public RabbitmqClient()
        {
        }
        

    }
}
