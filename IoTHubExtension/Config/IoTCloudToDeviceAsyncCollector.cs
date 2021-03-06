﻿using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices;
using System.Threading;

namespace IoTHubExtension.Config
{
    /// <summary>
    /// Provide the implementation for a collector.
    /// For the sample, we're writing <see cref="IoTCloudToDeviceItem"/>s to disk. 
    /// Collectors are used for emitting a series of discrete messages (ie, an output binding).
    /// </summary>
    public class IoTCloudToDeviceAsyncCollector : IAsyncCollector<IoTCloudToDeviceItem>
    {
        private static ServiceClient serviceClient;

        public IoTCloudToDeviceAsyncCollector(ServiceClient serviceClient, IoTCloudToDeviceAttribute attribute)
        {
            // create client;    
            IoTCloudToDeviceAsyncCollector.serviceClient = serviceClient;
        }
        public Task AddAsync(IoTCloudToDeviceItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            SendCloudToDeviceMessageAsync(item).Wait();
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        private async static Task SendCloudToDeviceMessageAsync(IoTCloudToDeviceItem item)
        {
            char[] messageCharArr = item.Message.ToCharArray();
            var deviceToCloudMessage = new Message(Encoding.ASCII.GetBytes(messageCharArr));
            await serviceClient.SendAsync(item.DeviceId, deviceToCloudMessage);
        }
    }
}
