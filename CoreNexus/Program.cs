using System;

namespace CoreNexus
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageHandler messageHandler = new MessageHandler();
            messageHandler.MessageRouting();
        }
    }
}
