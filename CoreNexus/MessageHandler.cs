using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreNexus
{
    //Class voor het handlen van inkomende en uitgaande berichten in de core
    public class MessageHandler
    {
        public MessageHandler()
        {
        }

        //Functie voor het creëren en routeren naar de juiste kanalen
        public void MessageRouting()
        {
            //Maak een instance can de RabbitMQClient aan
            RabbitmqClient client = new RabbitmqClient();

            //Maakt een connectie object doormiddel van de client.factory CreateConnection functie 
            using var connection = client.factory.CreateConnection();

            //Maakt de modellen waaruit de queues voorkomen
            using var uiInModel = connection.CreateModel();
            using var uiOutModel = connection.CreateModel();
            using var advInModel = connection.CreateModel();
            using var advOutModel = connection.CreateModel();

            //Maakt alle queues aan
            uiOutModel.QueueDeclare("UI-input-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            uiOutModel.QueueDeclare("UI-output-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            advInModel.QueueDeclare("AdviesTool-input-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            advOutModel.QueueDeclare("AdviesTool-output-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            //Zet een consumer op de UI-output-queue
            var uiOutConsumer = new EventingBasicConsumer(uiOutModel);
            uiOutConsumer.Received += (sender, e) =>
            {
                //haalt de array die de Jsonstring bevat uit de event
                var body = e.Body.ToArray();

                //zet deze array om naar een string waardoor alleen de jsonstring overblijft
                var message = Encoding.UTF8.GetString(body);

                //deserialze de jsonstring terug naar een object
                DatasetObject dObj = JsonConvert.DeserializeObject<DatasetObject>(message);

                //check om te kijken of de data die gestuurd is geworden in de lijst van het object toegangbaar is
                Console.WriteLine(dObj.data[0]);

                //check om te kijken of deze lijst muteerbaar is
                dObj.data.Add("core");
                Console.WriteLine(dObj.data[1]);
                var newBod = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dObj));
                advInModel.BasicPublish("", "AdviesTool-input-queue", null, newBod);
            };

            var advOutConsumer = new EventingBasicConsumer(advOutModel);
            advOutConsumer.Received += (sender, e) =>
            {
                //haalt de array die de Jsonstring bevat uit de event
                var body = e.Body.ToArray();

                //zet deze array om naar een string waardoor alleen de jsonstring overblijft
                var message = Encoding.UTF8.GetString(body);

                //deserialze de jsonstring terug naar een object
                DatasetObject dObj = JsonConvert.DeserializeObject<DatasetObject>(message);

                dObj.data.Add("Core");

                var newBod = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dObj));
                uiInModel.BasicPublish("", "UI-input-queue", null, newBod);
            };
            uiOutModel.BasicConsume("UI-output-queue", true, uiOutConsumer);
            advOutModel.BasicConsume("AdviesTool-output-queue", true, advOutConsumer);
            Console.ReadLine();

            //var uiMessage = uiOutModel.BasicConsume("UI-output-queue", true, uiOutConsumer);




            //Haalt de data uit de UI-output-queue en zet deze om naar JSON die weer doorgestuurd kan worden
            /*   var uiMessage = uiOutModel.BasicConsume("UI-output-queue", true, uiOutConsumer);

               if (uiMessage != null)
               {
                   Console.WriteLine(uiMessage);
                   var uiMsgBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(uiMessage));

                   if (uiMsgBody != null)
                   {
                       //Stuurt de message door naar de AdviesTool-input-queue
                       advInModel.BasicPublish("", "AdviesTool-input-queue", null, uiMsgBody);
                   }                
               }*/





            //Zet een consumer op de AdviesTool-output-queue
            //var advOutConsumer = new EventingBasicConsumer(advOutModel);
            //advOutConsumer.Received += (sender, e) =>
            //{
            //    var body = e.Body;
            //};

            //Haalt de data uit de AdviesTool-output-queue en zet deze om naar JSON die weer doorgestuurd kan worden
            //var advMessage = advOutModel.BasicConsume("AdviesTool-output-queue", true, advOutConsumer);

            //if (advMessage != null)
            //{
            //Console.WriteLine(advMessage);
            //var advMsgBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(uiMessage));
            //if (advMsgBody != null)
            //{
            //Stuurt de message door naar de UI-input-queue
            //uiInModel.BasicPublish("", "UI-input-queue", null, advMsgBody);
            //}

            //}


        }

    }


}
