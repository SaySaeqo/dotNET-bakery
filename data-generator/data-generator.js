console.log('Start of the script');
const mqtt = require('mqtt');
const fs = require('fs');
const topic = 'sensor/data';
const sensorTypes = ['temperature', 'weight', 'gas_density', 'humidity'];

// Read the sensor configuration from the config file
const config = JSON.parse(fs.readFileSync('config.json', 'utf8'));
const sensors = config.sensors;

// Connect to the RabbitMQ server
console.log('Attempting to connect to RabbitMQ server...');
const client = mqtt.connect('mqtt://rabbitmq:1883');

client.on('connect', function () {
  console.log('Successfully connected to RabbitMQ server.');
  console.log('Starting data generation...');
  client.subscribe([topic], () => {
    console.log(`Subscribe to topic '${topic}'`)
  })

  // Start data generation for each sensor
  for (let sensor of sensors) {
    setInterval(() => generateData(sensor), Math.floor(60/sensor.timesPerMinute * 1000));
  }
});

client.on('error', function (err) {
  console.error('An error occurred:', err);
});

function generateData(sensor) {
  const data = {
    id: sensor.id,
    date: new Date(),
    value: Math.floor(Math.random() * (sensor.maxValue - sensor.minValue + 1)) + sensor.minValue,
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
