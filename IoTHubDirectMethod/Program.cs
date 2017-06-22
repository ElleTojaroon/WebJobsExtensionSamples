using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace IoTHubDirectMethodExtension
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=IotHubC2D.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=nYvvF3M9jXESW2lH/18BgZ5bSjXqdhwH3BcGqBBGh2k=";      

        static void Main(string[] args)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            InvokeMethod("myFirstDevice", "writeLine").Wait();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static async Task InvokeMethod(string deviceID, string method)
        {
            var methodInvocation = new CloudToDeviceMethod(method) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson("'a line to be written'");

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceID, methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }
    }
}
