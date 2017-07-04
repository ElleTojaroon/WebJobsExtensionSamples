﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;

namespace IoTHubExtension.Config
{
    public class IoTCloudToDeviceExtension : IExtensionConfigProvider
    {
        private static string connectionString;
        private static ServiceClient serviceClient;

        public void Initialize(ExtensionConfigContext context)
        {
            connectionString = Environment.GetEnvironmentVariable("IoTConnectionString");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            // This allows a user to bind to IAsyncCollector<string>, and the sdk
            // will convert that to IAsyncCollector<IoTCloudToDeviceItem>
            context.AddConverter<string, IoTCloudToDeviceItem>(ConvertToItem);

            // This is useful on input. 
            context.AddConverter<IoTCloudToDeviceItem, string>(ConvertToString);

            // Create 2 binding rules for the Sample attribute.
            var rule = context.AddBindingRule<IoTCloudToDeviceAttribute>();

            //rule.BindToInput<SampleItem>(BuildItemFromAttr);
            rule.BindToCollector<IoTCloudToDeviceItem>(BuildCollector);
        }
        
        private string ConvertToString(IoTCloudToDeviceItem item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private IoTCloudToDeviceItem ConvertToItem(string str)
        {
            var item = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            return new IoTCloudToDeviceItem
            {
                DeviceId = item["DeviceId"],
                MessageId = item["MessageId"],
                Message = str
            };
        }

        private IAsyncCollector<IoTCloudToDeviceItem> BuildCollector(IoTCloudToDeviceAttribute attribute)
        {
            return new IoTCloudToDeviceAsyncCollector(serviceClient, attribute);
        }
    }
}