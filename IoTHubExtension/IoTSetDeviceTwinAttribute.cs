using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;

namespace IoTHubExtension
{
    /// <summary>
    /// Binding attribute to place on user code for WebJobs. 
    /// </summary>
    [Binding]
    public class IoTSetDeviceTwinAttribute : Attribute
    {
        [AutoResolve]
        public string DeviceId { get; set; }

        [AutoResolve]
        public string UpdateId { get; set; }

        [AutoResolve]
        public string Patch { get; set; }

        [AppSetting(Default = "IoTConnectionString")]
        public string ConnectionString { get; set; }
    }
}
