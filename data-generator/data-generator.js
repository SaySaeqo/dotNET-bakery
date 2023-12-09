console.log('Start of the script');
const mqtt = require('mqtt');
const timesPerMinute = 5;
const topic = 'sensor/data';
const sensorTypes = ['temperature', 'weight', 'gas_density', 'humidity'];

// Connect to the RabbitMQ server
console.log('Attempting to connect to RabbitMQ server...');
const client = mqtt.connect('mqtt://rabbitmq:1883');

client.on('connect', function () {
  console.log('Successfully connected to RabbitMQ server.');
  console.log('Starting data generation every second...');
  client.subscribe([topic], () => {
    console.log(`Subscribe to topic '${topic}'`)
    // client.publish(topic, 'nodejs mqtt test', { qos: 0, retain: false }, (error) => {
    //   if (error) {
    //     console.error(error)
    //   }
    // })
  })
  setInterval(generateData, timesPerMinute * 1000);
});

// client.on('message', (topic, payload) => {
//     console.log('Received Message:', topic, payload.toString())
//   })

client.on('error', function (err) {
  console.error('An error occurred:', err);
});

function generateData() {
  for (let i = 1; i <= 1; i++) {
    const data = {
      id: i,
      date: new Date(),
      value: Math.floor(Math.random() * 100),
      type: sensorTypes[Math.floor((i - 1) / 4)],
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
}
