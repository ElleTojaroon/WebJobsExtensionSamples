'use strict';
var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
var connectionString = 'HostName=ElleIoTHubFinalTest1.azure-devices.net;DeviceId=receiverBob;SharedAccessKey=YquAgK0RYj5+zeXtGsFZgr7JdnnZ73weW/im5QhOPPk=';
var client = clientFromConnectionString(connectionString);

// GPIO pin of the led
var wpi = require('wiringpi-node');
var configPin = 7;
wpi.setup('wpi');
wpi.pinMode(configPin, wpi.OUTPUT);
var isLedOn = 0;
var stillMove = 0;
var yellowColor = "\x1b[33m%s\x1b[0m:"; // yellow -telemetry to print only
var redColor = "\x1b[31m"; // red -urgent
var resetFontColor = "\x1b[0m";

function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res.constructor.name);
    };
}

var onPeopleMove = function () {
    stillMove += 1;
    isLedOn = 1;
	wpi.digitalWrite(configPin, isLedOn );
    console.log(yellowColor, "LED's on", resetFontColor);
    watcher();
    setTimeout(function(){
        if (!stillMove) {
            console.log(redColor, "LED's off", resetFontColor);
            isLedOn = 0;
            wpi.digitalWrite(configPin, isLedOn );
        }
     }, 10500);
}

var watcher = function() {
    setTimeout(function(){
        stillMove = 0;
     }, 10000);
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
