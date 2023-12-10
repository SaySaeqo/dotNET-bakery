console.log('Start of the script');
const mqtt = require('mqtt');
const fs = require('fs');
const gaze = require('gaze');
const topic = 'sensor/data';
const sensorTypes = ['temperature', 'weight', 'gas_density', 'humidity'];

let sensors = [];
let intervals = [];

// Connect to the RabbitMQ server
console.log('Attempting to connect to RabbitMQ server...');
const client = mqtt.connect('mqtt://rabbitmq:1883');

client.on('connect', function () {
  console.log('Successfully connected to RabbitMQ server.');
  console.log('Starting data generation...');
  client.subscribe([topic], () => {
    console.log(`Subscribe to topic '${topic}'`)
  })

  // Load the sensor configuration and start data generation
  loadConfigAndStartGeneration();
});

client.on('error', function (err) {
  console.error('An error occurred:', err);
});

function loadConfigAndStartGeneration() {
  // Clear all existing intervals
  for (let interval of intervals) {
    clearInterval(interval);
  }
  intervals = []; // Reset the intervals array

  // Read the sensor configuration from the config file
  const config = JSON.parse(fs.readFileSync('config.json', 'utf8'));
  sensors = config.sensors;

  // Start data generation for each sensor
  for (let sensor of sensors) {
    let interval = setInterval(() => generateData(sensor), Math.floor(60/sensor.timesPerMinute * 1000));
    intervals.push(interval); // Store the interval ID
  }
}

function generateData(sensor) {
  let value;
  if (sensor.setValue !== null) {
    value = sensor.setValue;
  } else {
    value = Math.floor(Math.random() * (sensor.maxValue - sensor.minValue + 1)) + sensor.minValue;
  }

  const data = {
    id: sensor.id,
    date: new Date(),
    value: value,
    type: sensorTypes[(sensor.id - 1) % sensorTypes.length],
  };

  // Publish the data to the RabbitMQ server
  console.log('Attempting to publish data:', data);
  client.publish(topic, JSON.stringify(data), function (err) {
    if (err) {
      console.error('An error occurred while publishing:', err);
    } else {
      console.log('Data successfully sent: ', data);
    }
  });
}

// Watch for changes in the config file using gaze
gaze('config.json', function(err, watcher) {
  this.on('changed', function(filepath) {
    console.log(`config.json has been updated. Reloading configuration...`);
    loadConfigAndStartGeneration();
  });
});
