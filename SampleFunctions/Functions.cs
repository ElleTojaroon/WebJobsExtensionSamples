using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using IoTHubExtension;
using System.IO;
using Newtonsoft.Json;

namespace SampleFunctions
{
    public class Functions
    {
        // Write some messages
        [NoAutomaticTrigger]
        public void WriteMessageFromC2D([IoTCloudToDevice] ICollector<string> output)
        {
            var item =  new
            {
                DeviceId = "myFirstDevice",
                MessageId = "1",
                Message = "Hello"
            };
            output.Add(JsonConvert.SerializeObject(item));

            item = new 
            {
                DeviceId = "myFirstDevice",
                MessageId = "2",
                Message = "From"
            };
            output.Add(JsonConvert.SerializeObject(item));

            item = new 
            {
                DeviceId = "myFirstDevice",
                MessageId = "3",
                Message = "Cloud"
            };
            output.Add(JsonConvert.SerializeObject(item));
        }

        // Write some messages
        [NoAutomaticTrigger]
        public void DirectInvokeMethod([IoTDirectMethod] ICollector<string> output)
        {
            var item = new
            {
                DeviceId = "myFirstDevice",
                InvokeId = "1",
                MethodName = "writeLine"
            };
            output.Add(JsonConvert.SerializeObject(item));

            item = new
            {
                DeviceId = "myFirstDevice",
                InvokeId = "2",
                MethodName = "writeLine"
            };
            output.Add(JsonConvert.SerializeObject(item));

            item = new
            {
                DeviceId = "myFirstDevice",
                InvokeId = "3",
                MethodName = "writeLine"
            };
            output.Add(JsonConvert.SerializeObject(item));
        }

#if false
        #region Using 2nd extensions

        // Bind to input as rich type:
        // BindToInput<SampleItem> --> item
        [NoAutomaticTrigger]
        public void Reader3(
            string name,  // from trigger
            [Sample(Name = "{name}")] CustomType<int> item,
            TextWriter log)
        {
            log.WriteLine($"Via custom type {item.Name}:{item.Value}");
        }
        #endregion
#endif
    }
}
