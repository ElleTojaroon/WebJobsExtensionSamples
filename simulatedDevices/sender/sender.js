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

var connectCallback = function (err) {
  if (err) {
    console.log('Could not connect: ' + err);
  } else {
    console.log('Client connected');

    // Create a message and send it to the IoT Hub every second
    setInterval(function(){  
        var isDirectMethod = false;
        var messageString;
        var fontColor;
        var receiverDeviceId;

        if (count%3 == 0) {
            isDirectMethod = true;
            messageString = 'writeLine';
            receiverDeviceId = 'receiverAlice';
            fontColor = "\x1b[31m"; // red - urgent
        } else {
            messageString = "telemetry data point";
            receiverDeviceId = 'receiverBob';
            fontColor = "\x1b[33m%s\x1b[0m:"; // yellow
        }

        var data = JSON.stringify({ DeviceId: receiverDeviceId, MessageId: Date.now(), Message: messageString });
        var message = new Message(data);
        message.properties.add('isDirectMethod', isDirectMethod);

        console.log("Sending message: " + fontColor, message.getData(), "\x1b[0m");
        client.sendEvent(message, printResultFor('send'));
        count += 1;
    }, 5000);
  }
};

client.open(connectCallback);