using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using IoTHubExtension;
using System.IO;
using Newtonsoft.Json;
using System;

namespace SampleFunctions
{
    public class Functions
    {
        // Write some messages
        [NoAutomaticTrigger]
        public void WriteMessageFromC2D([IoTCloudToDevice] ICollector<string> output)
        {
            var item = new
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

        // Write some messages
        [NoAutomaticTrigger]
        public void SetDeviceTwin([IoTSetDeviceTwin] ICollector<string> output)
        {

            var item2 = new
            {
                DeviceId = "receiverAlice",
                UpdateId = "2",
                Patch = new
                {
                    properties = new
                    {
                        desired = new
                        {
                            telemetryConfig = new
                            {
                                configId = Guid.NewGuid().ToString()
                            }
                        }
                    }
                }
            };
            output.Add(JsonConvert.SerializeObject(item2));

            var item = new
            {
                DeviceId = "receiverAlice",
                UpdateId = "1",
                Patch = new
                {
                    tags = new
                    {
                        location = new
                        {
                            region = "US", 
                            plant = "Redmond43"
                        }
                    }
                }
            };
            output.Add(JsonConvert.SerializeObject(item));

            var item3 = new
            {
                DeviceId = "receiverAlice",
                UpdateId = "3",
                Patch = new
                {
                    properties = new
                    {
                        desired = new
                        {
                            telemetryConfig = new
                            {
                                configId = Guid.NewGuid().ToString()
                            }
                        }
                    }
                }
            };
            output.Add(JsonConvert.SerializeObject(item3));
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
