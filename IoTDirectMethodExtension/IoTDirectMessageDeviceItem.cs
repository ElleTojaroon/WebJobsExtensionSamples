using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDirectMethodExtension
{
    public class IoTDirectMessageDeviceItem
    {
        // Destination IoT DeviceId
        public string DeviceId { set; get; }

        // InvokeId starting with 1 per DeviceId
        public string InvokeId { set; get; }

        // MethodName to be invoked
        public string MethodName { set; get; }
    }
}
