'use strict';

var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
var connectionString = 'HostName=IotHubC2D.azure-devices.net;DeviceId=sender;SharedAccessKey=8r2cSkjX3uCMdXXS7xVEyjylF6QipA9p814NuE37Koo=';
var client = clientFromConnectionString(connectionString);
var count = 0;

function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res.constructor.name);
    };
}

// var initConfigChange = function (twin) {
//     var currentTelemetryConfig = twin.properties.reported.telemetryConfig;
//     currentTelemetryConfig.pendingConfig = twin.properties.desired.telemetryConfig;
//     currentTelemetryConfig.status = "\x1b[36m Pending \x1b[0m";

//     var patch = {
//         telemetryConfig: currentTelemetryConfig
//     };
//     twin.properties.reported.update(patch, function (err) {
//         if (err) {
//             console.log('\x1b[36m  Could not report properties \x1b[0m');
//         } else {
//             console.log('\x1b[36m Reported pending config change: ' + JSON.stringify(patch) + " \x1b[0m");
//             setTimeout(function () { completeConfigChange(twin); }, 60000);
//         }
//     });
// }
// var completeConfigChange = function (twin) {
//     var currentTelemetryConfig = twin.properties.reported.telemetryConfig;

//     try {
//         currentTelemetryConfig.configId = currentTelemetryConfig.pendingConfig.configId;
//         currentTelemetryConfig.sendFrequency = currentTelemetryConfig.pendingConfig.sendFrequency;
//         currentTelemetryConfig.status = "\x1b[36m Success \x1b[0m";
//         delete currentTelemetryConfig.pendingConfig;

//         var patch = {
//             telemetryConfig: currentTelemetryConfig
//         };
//         patch.telemetryConfig.pendingConfig = null;

//         twin.properties.reported.update(patch, function (err) {
//             if (err) {
//                 console.error('\x1b[36m Error reporting properties: ' + err + " \x1b[0m");
//             } else {
//                 console.log('\x1b[36m Reported completed config change: ' + JSON.stringify(patch) + " \x1b[0m");
//             }
//         });
//     } catch (error) {
//         console.log("Fails to complete setting device twin. Check the currect config to see if your change went through");
//         console.log(currentTelemetryConfig)
//     }
// }

var connectCallback = function (err) {
    if (err) {
        console.log('Could not connect: ' + err);
    } else {
        console.log('Client connected');

        // Create a message and send it to the IoT Hub every second
        setInterval(function () {
            var isDirectMethod = false;
            var messageString;
            var fontColor;
            var receiverDeviceId;

            if (count % 5 == 0) {
                isDirectMethod = true;
                messageString = 'writeLine';
                receiverDeviceId = 'receiverAlice';
                fontColor = "\x1b[31m"; // red -urgent
            } else {
                messageString = "telemetry data point";
                receiverDeviceId = 'receiverBob';
                fontColor = "\x1b[33m%s\x1b[0m:"; // yellow -telemetry
            }

            var data = JSON.stringify({ DeviceId: receiverDeviceId, MessageId: Date.now(), Message: messageString });
            var message = new Message(data);
            message.properties.add('isDirectMethod', isDirectMethod);

            console.log("Sending message: " + fontColor, message.getData(), "\x1b[0m");
            client.sendEvent(message, printResultFor('send'));
            count += 1;
        }, 5000);

        // client.getTwin(function (err, twin) {
        //     if (err) {
        //         console.error('\x1b[36m could not get twin \x1b[0m');
        //     } else {
        //         console.log('\x1b[36m retrieved device twin \x1b[0m');
        //         twin.properties.reported.telemetryConfig = {
        //             configId: "0",
        //             sendFrequency: "24h"
        //         }
        //         twin.on('properties.desired', function (desiredChange) {
        //             console.log("\x1b[36m received device twin change: " + JSON.stringify(desiredChange) + " \x1b[0m");
        //             var currentTelemetryConfig = twin.properties.reported.telemetryConfig;
        //             if (desiredChange.telemetryConfig && desiredChange.telemetryConfig.configId !== currentTelemetryConfig.configId) {
        //                 initConfigChange(twin);
        //             }
        //         });
        //     }
        // });
    }
};

client.open(connectCallback);