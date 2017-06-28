using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "IotHubC2D.azure-devices.net";
        static string deviceKey = "Vi+e5SqkjxVVKmDJVnFhmBhoQS0V0Rg8aeSsVCAV4vE=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("mySecondDevice", deviceKey), TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();

            ReceiveC2dAsync();

            Console.ReadLine();
        }

        // when device receives data from cloud
        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received message: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // input data from device
                var telemetryDataPoint = new
                {
                    MessageId = messageId++,
                    DeviceId = "myFirstDevice",
                    TimeStamp = DateTime.Now,
                    Temperature = currentTemperature,
                    Humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                Console.ResetColor();

                await Task.Delay(5000);


            }
        }
    }
}
