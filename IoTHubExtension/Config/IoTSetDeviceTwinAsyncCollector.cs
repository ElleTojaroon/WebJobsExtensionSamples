using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices;
using System.Threading;
using System;
using Newtonsoft.Json;

namespace IoTHubExtension.Config
{
    public class IoTSetDeviceTwinAsyncCollector : IAsyncCollector<IoTSetDeviceTwinItem>
    {
        static RegistryManager registryManager;
        
        public IoTSetDeviceTwinAsyncCollector(IoTSetDeviceTwinAttribute attribute)
        {
            // create client;
            registryManager = RegistryManager.CreateFromConnectionString(attribute.ConnectionString);            
        }

        public Task AddAsync(IoTSetDeviceTwinItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            SetDesiredConfigurationAndQuery(item).Wait();
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        static private async Task SetDesiredConfigurationAndQuery(IoTSetDeviceTwinItem item)
        {
            var twin = await registryManager.GetTwinAsync(item.DeviceId);
            await registryManager.UpdateTwinAsync(twin.DeviceId, item.Patch, twin.ETag);
            Console.WriteLine("Updated desired configuration");            
        }
    }
}
