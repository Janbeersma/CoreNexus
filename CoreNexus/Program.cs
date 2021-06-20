using System;

namespace CoreNexus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MessageHandler messageHandler = new MessageHandler();
            messageHandler.MessageRouting();
        }
    }
}
