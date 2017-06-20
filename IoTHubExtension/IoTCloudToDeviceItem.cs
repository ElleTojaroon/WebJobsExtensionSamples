using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubExtension
{
    /// <summary>
    /// A "native type" for the extension. This gives full access to the extension's object model. 
    /// The extension may then register converters to convert between this and "bcl" types 
    /// like string, JObject, etc. 
    /// </summary>
    public class IoTCloudToDeviceItem
    {
        // IoT DeviceId
        public string DeviceId { set; get; }
        
        // Messege sent from device to cloud
        public string Message { set; get; }
    }
}
