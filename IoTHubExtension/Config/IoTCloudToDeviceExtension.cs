using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace IoTHubExtension.Config
{
    public class IoTCloudToDeviceExtension : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
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
            return item.Message;
        }

        private IoTCloudToDeviceItem ConvertToItem(string arg)
        {
            var parts = arg.Split(':');
            return new IoTCloudToDeviceItem
            {
                DeviceId = parts[0],
                Message = parts[1]
            };
        }

        private IAsyncCollector<IoTCloudToDeviceItem> BuildCollector(IoTCloudToDeviceAttribute attribute)
        {
            return new IoTCloudToDeviceAsyncCollector(attribute);
        }
    }
}