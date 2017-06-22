using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices;
using System.Threading;
using System;

namespace IoTDirectMethodExtension.Config
{
    public class IoTDirectMethodAsyncCollector : IAsyncCollector<IoTDirectMessageDeviceItem>
    {
        private static ServiceClient serviceClient;

        public IoTDirectMethodAsyncCollector(IoTDirectMethodDeviceAttribute attribute)
        {
            // create client;
            serviceClient = ServiceClient.CreateFromConnectionString(attribute.ConnectionString);
        }
        public Task AddAsync(IoTDirectMessageDeviceItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            InvokeMethod(item.DeviceId, item.MethodName).Wait();
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        private async static Task SendCloudToDeviceMessageAsync(IoTDirectMessageDeviceItem item)
        {
            char[] messageCharArr = item.MethodName.ToCharArray();
            var deviceToCloudMessage = new Message(Encoding.ASCII.GetBytes(messageCharArr));
            await serviceClient.SendAsync(item.DeviceId, deviceToCloudMessage);
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
