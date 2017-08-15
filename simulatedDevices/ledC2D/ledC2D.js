'use strict';
var wpi = require('wiringpi-node');

var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
var connectionString = 'HostName=ElleIoTHubFinalTest1.azure-devices.net;DeviceId=receiverBob;SharedAccessKey=YquAgK0RYj5+zeXtGsFZgr7JdnnZ73weW/im5QhOPPk=';
var client = clientFromConnectionString(connectionString);

// GPIO pin of the led
var configPin = 7;
wpi.setup('wpi');
wpi.pinMode(configPin, wpi.OUTPUT);
var isLedOn = 0;

function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res.constructor.name);
    };
}

var onPeopleMove = function () {
    isLedOn = 1;
	wpi.digitalWrite(configPin, isLedOn );
    setTimeout(function(){ 
        isLedOn = 0;
        wpi.digitalWrite(configPin, isLedOn );  
     }, 3000);
}

var connectCallback = function (err) {
    if (err) {
        console.log('Could not connect: ' + err);
    } else {
        console.log('Client connected');

        client.on('message', function (msg) {
            onPeopleMove();
            console.log(' Message: ' + msg.data);
            client.complete(msg, printResultFor('completed'));
        });
    }
};

client.open(connectCallback);
