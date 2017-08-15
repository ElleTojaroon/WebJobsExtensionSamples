'use strict';

var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
var connectionString = 'HostName=ElleIoTHubFinalTest1.azure-devices.net;DeviceId=sender;SharedAccessKey=wAcLzS9NC8IipECzeO5lF3p+e41ePmGkou4lq0vgoig=';
var client = clientFromConnectionString(connectionString);

var messageString = "telemetry data point";
var receiverDeviceId = 'receiverBob'; 
var fontColor = "\x1b[33m%s\x1b[0m:"; // yellow -telemetry to print only
var resetFontColor = "\x1b[0m";

function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res.constructor.name);
    };
}

var connectCallback = function (err) {
    if (err) {
        console.log('Could not connect: ' + err);
    } else {
        console.log('Client connected');

        // Create a message and send it to the IoT Hub every 5 second
        setInterval(function () {            
            var data = JSON.stringify({ DeviceId: receiverDeviceId, Message: messageString });
            var message = new Message(data);
            console.log("Sending message: " + fontColor, message.getData(), resetFontColor);
            client.sendEvent(message, printResultFor('send'));
        }, 5000);
    }
};

client.open(connectCallback);