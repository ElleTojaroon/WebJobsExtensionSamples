// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs;
using SampleFunctions;
using System;

namespace Host
{
    // WebJobs is .NET 4.6 
    class Program
    {
        static void Main(string[] args)
        {
            var config = new JobHostConfiguration();
            config.DashboardConnectionString = null;

            // apply config before creating the host.
            var clouldToDeviceExtension = new IoTHubExtension.Config.IoTCloudToDeviceExtension();
            config.AddExtension(clouldToDeviceExtension);            

            // Debug diagnostics!
            config.CreateMetadataProvider().DebugDumpGraph(Console.Out);

            var host = new JobHost(config);

            // Test some invocations. 
            var method = typeof(Functions).GetMethod("WriteMessageFromC2D");
            host.Call(method);
            
            // host.RunAndBlock();
        }
    }
}
