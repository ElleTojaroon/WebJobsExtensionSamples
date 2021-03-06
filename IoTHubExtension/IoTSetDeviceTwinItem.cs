﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubExtension
{
    public class IoTSetDeviceTwinItem
    {
        // Destination IoT DeviceId
        public string DeviceId { set; get; }

        // InvokeId starting with 1 per DeviceId
        public string UpdateId { set; get; }

        // new configuration to change (either tags or desired properties)
        public string Patch { set; get; }
    }
}
