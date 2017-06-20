using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SampleExtension;
using IoTHubExtension;
using System.IO;

namespace SampleFunctions
{
    // USe the Sample Extension 
    public class Functions
    {
        /*
        // Write some messages
        [NoAutomaticTrigger]
        public void Writer([Sample] ICollector<string> output)
        {
            // Each string gets converted to a SampleItem and then emited. 
            output.Add("bob:10");
            output.Add("joe:11");
            output.Add("tom:12");
        }

        */
        // Write some messages
        [NoAutomaticTrigger]
        public void WriteMessageFromC2D([IoTCloudToDevice] ICollector<string> output)
        {
            // Each string gets converted to a SampleItem and then emited. 
            output.Add("myFirstDevice:Hello");
            output.Add("myFirstDevice:From");
            output.Add("myFirstDevice:Cloud");
        }

        /*
        // Bind to input as string
        // BindToInput<SampleItem> --> Converter --> string
        [NoAutomaticTrigger]
        public void ReadMessageFromC2D(
            string deviceId,  // from trigger,
            [IoTCloudToDevice(DeviceId = "{deviceId}")] IoTCloudToDeviceItem item,
            TraceWriter log)
        {
            log.Info($"{item.DeviceId}:{item.Message}");
        }

    */

        // Bind to input as string
        // BindToInput<SampleItem> --> Converter --> string
        [NoAutomaticTrigger]
        public void Reader(
            string name,  // from trigger
            [Sample(FileName = "{name}")] string contents, 
            TraceWriter log)
        {
            log.Info(contents);
        }

        // Bind to input as rich type:
        // BindToInput<SampleItem> --> item
        [NoAutomaticTrigger]
        public void Reader2(
            string name,  // from trigger
            [Sample(FileName = "{name}")] SampleItem item,
            [Sample] ICollector<string> output,
            TextWriter log)
        {
            output.Add($"{item.Name}:{item.Contents}");
            log.WriteLine($"{item.Name}:{item.Contents}");
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
