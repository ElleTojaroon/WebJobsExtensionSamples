using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDirectMethodExtension.Config
{
    public class IoTDirectMethodExtension : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            // This allows a user to bind to IAsyncCollector<string>, and the sdk
            // will convert that to IAsyncCollector<IoTCloudToDeviceItem>
            context.AddConverter<string, IoTDirectMessageDeviceItem>(ConvertToItem);

            // This is useful on input. 
            context.AddConverter<IoTDirectMessageDeviceItem, string>(ConvertToString);

            // Create 2 binding rules for the Sample attribute.
            var rule = context.AddBindingRule<IoTDirectMethodDeviceAttribute>();

            //rule.BindToInput<SampleItem>(BuildItemFromAttr);
            rule.BindToCollector<IoTDirectMessageDeviceItem>(BuildCollector);
        }

        private string ConvertToString(IoTDirectMessageDeviceItem item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private IoTDirectMessageDeviceItem ConvertToItem(string str)
        {
            var item = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            return new IoTDirectMessageDeviceItem
            {
                DeviceId = item["DeviceId"],
                InvokeId = item["InvokeId"],
                MethodName = item["MethodName"]
            };
        }

        private IAsyncCollector<IoTDirectMessageDeviceItem> BuildCollector(IoTDirectMethodDeviceAttribute attribute)
        {
            return new IoTDirectMethodAsyncCollector(attribute);
        }
    }
}