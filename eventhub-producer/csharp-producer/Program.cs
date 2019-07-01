using System;

namespace csharpeventhub
 {
     using System;
     using System.Text;
     using System.Threading.Tasks;
     using Microsoft.Azure.EventHubs;

     public class Program
     {
         private static EventHubClient eventHubClient;
         private const string EventHubNamespace = "sample-eventhub-namespace";
         private const string EventHubName = "sample-eventhub";
         private const string EventHubKey = "<your-eventhub-key>";
         private const string EventHubConnectionString = "Endpoint=sb://{0}.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={1}";

         public static void Main(string[] args)
         {
             MainAsync(args).GetAwaiter().GetResult();
         }

         private static async Task MainAsync(string[] args)
         {
             // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
             // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
             // we are using the connection string from the namespace.
             var connectionStringBuilder = new EventHubsConnectionStringBuilder(String.Format(EventHubConnectionString, EventHubNamespace, EventHubKey))
             {
                 EntityPath = EventHubName
             };

             eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            // Sending 50 messages to Event Hub
             await SendMessagesToEventHub(50);
             await eventHubClient.CloseAsync();

             Console.WriteLine("Press ENTER to exit.");
             Console.ReadLine();
         }

         // Creates an event hub client and sends 100 messages to the event hub.
         private static async Task SendMessagesToEventHub(int numMessagesToSend)
         {
             for (var i = 0; i < numMessagesToSend; i++)
             {
                 try
                 {
                     var message = $"Message {i}";
                     Console.WriteLine($"Sending message: {message}");
                     await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                 }
                 catch (Exception exception)
                 {
                     Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                 }

                 await Task.Delay(10);
             }

             Console.WriteLine($"{numMessagesToSend} messages sent.");
         }
     }
 }