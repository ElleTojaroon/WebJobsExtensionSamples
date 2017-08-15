'use strict';

var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
// var connectionString = 'HostName=IotHubC2D.azure-devices.net;DeviceId=sender;SharedAccessKey=8r2cSkjX3uCMdXXS7xVEyjylF6QipA9p814NuE37Koo=';  // IoTHubC2D
// var connectionString = 'HostName=Elle2ndIoTHub.azure-devices.net;DeviceId=sender;SharedAccessKey=thN78AdNhWdSaJt5lR3b/xcj/1cUp4OC3Z9pUtsnrEQ=' // Elle2ndIoTHub
// var connectionString = 'HostName=Elle3rdIOTHub.azure-devices.net;DeviceId=sender;SharedAccessKey=J5yIqFltPA1RgOIChFa6T82UkK32GYBNX4WetKFZkNE=';
var connectionString = 'HostName=ElleIoTHubFinalTest1.azure-devices.net;DeviceId=sender;SharedAccessKey=wAcLzS9NC8IipECzeO5lF3p+e41ePmGkou4lq0vgoig=';
var client = clientFromConnectionString(connectionString);
var count = 0;

var forAllTelemetry = 1;
var forAllDirectMethods = 0;
var forAllSBQueue = 0;
var forAllSBTopic = 0;
var forAnyAll = forAllTelemetry || forAllDirectMethods || forAllSBQueue || forAllSBTopic;

var forSomeTelemetry = 0;
var forSomeDirectMethod = 0;
var forSomeSBQueue = 0;
var forSomeSBTopic = 0;

// GPIO pin of the led
// var configPin = 10;
// wpi.setup('wpi');
// wpi.pinMode(configPin, wpi.OUTPUT);
// var isLedOn = 0;

// var blinkLED = function () {
//     isLedOn = 1;
// 	wpi.digitalWrite(configPin, isLedOn );
//     setTimeout(function(){ 
//         isLedOn = 0;
//         wpi.digitalWrite(configPin, isLedOn );  
//      }, 1000);
// }

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
            var isSBQueue = false;
            var isSBTopic = false;
            var messageString;
            var fontColor;
            var receiverDeviceId;

            if (forAllDirectMethods || (!forAnyAll && forSomeDirectMethod && count % 5 == 0)) {
                isDirectMethod = true;
                messageString = 'writeLine';
                receiverDeviceId = 'receiverAlice';
                fontColor = "\x1b[31m"; // red -urgent
            } else if (forAllSBQueue || (!forAnyAll && forSomeSBQueue && count % 3 == 0)) {
                isSBQueue = true;
                messageString = 'writeLine';
                receiverDeviceId = 'receiverAlice';
                fontColor = "\x1b[32m"; // green
            } else if (forAllSBTopic || (!forAnyAll && forSomeSBTopic && count % 7 == 0)) {
                isSBTopic = true;
                messageString = 'writeLine';
                receiverDeviceId = 'receiverAlice';
                fontColor = "\x1b[36m"; // cyan
            } else if (forAllTelemetry || (!forAnyAll && forSomeTelemetry)) { // isAllTelemetry
                messageString = "telemetry data point";
                receiverDeviceId = 'receiverBob'; // used to be receiverBob
                fontColor = "\x1b[33m%s\x1b[0m:"; // yellow -telemetry
            }

            var data = JSON.stringify({ DeviceId: receiverDeviceId, MessageId: Date.now(), Message: messageString });
            var message = new Message(data);
            message.properties.add('isDirectMethod', isDirectMethod); //isDirectMethod
            message.properties.add('isSBQueue', isSBQueue);
            message.properties.add('isSBTopic', isSBTopic);

            // message.properties.add('elle3rdTestEHRoute1', true);
            // message.properties.add('elle3rdTestEHRoute2', true);
            // message.properties.add('elle3rdTestSBQueueRoute1', true);
            // message.properties.add('elle3rdTestSBQueueRoute2', true);
            // message.properties.add('elle3rdTestSBTopicRoute1', true);
            // message.properties.add('elle3rdTestSBTopicRoute2', true);

            console.log('isDirectMethod', isDirectMethod);
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