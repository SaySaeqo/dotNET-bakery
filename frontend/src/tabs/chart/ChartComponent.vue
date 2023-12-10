<template>
    <div class="px-16">
        <v-row>
            <v-col v-for="sensor of Object.values(sensors)" v-bind:key="sensor.id">
                <v-card 
                    variant="outlined" 
                    elevation="16" 
                    :style="`border: 2px solid ${sensor.color}`"
                >
                    <v-card-item>
                        <v-card-title>Sensor {{sensor.id}}</v-card-title>
                    </v-card-item>
                    <v-card-text>
                        <p>Last: {{sensor.last}}</p>
                        <p>Average: {{sensor.average}}</p>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
        <v-row>
            <Line :data="data" :options="options" />
        </v-row>      
    </div>
</template>

<script>
import {
    Chart as ChartJS,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    TimeSeriesScale,
} from 'chart.js';
import { Line } from 'vue-chartjs';
import 'chartjs-adapter-moment';
import MeasurementsService from '@/service/MeasurementsService';
import ChartDataBuilder from '@/utils/ChartDataBuilder';

ChartJS.register(
    LinearScale,
    PointElement,
    LineElement,
    Title,
    TimeSeriesScale
);

export default {
    name: 'ChartComponent',
    components: { Line },
    props: {
        type: {
            type: String,
            required: true
        },
        title: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            sensors: {},
            getSensorsDataInterval: null,
            data: {
                datasets: [],
                labels: []
            },
            options: {
                layout: {
                    padding: 10,
                },
                plugins: {
                    title: {
                        display: true,
                        text: this.title
                    }
                },
                scales: {
                    x: {
                        type: 'timeseries',
                        time: {
                            tooltipFormat: 'DD T'
                        },
                        title: {
                            display: true,
                            text: 'Date'
                        },
                        ticks: {
                            source: 'data'
                        }
                    },
                    y: {
                        title: {
                            display: true,                          
                        }
                    }
                },
            }
        };
    },
    async created() {
        await this.getData();
        await this.getSensorsData();
    },
    mounted() {
        this.getSensorsDataInterval = setInterval(() => {
            this.getSensorsData();
        }, 500);
    },
    beforeUnmount() {
        clearInterval(this.getSensorsDataInterval);
    },
    methods: {
        async getData() {
            this.options.scales.y.title.text = this.type;
            MeasurementsService.get({ types: [this.type], sortBy: 'date' })
                .then((response) => {
                    const datasets = ChartDataBuilder.build(response.data);
                    this.data = { datasets: Object.values(datasets) };
                    Object.keys(datasets).forEach(key => {
                        this.sensors[key] = { color: datasets[key].borderColor };
                    });
                }).
                catch((error) => {
                    console.log(error);
                });
        },
        async getSensorsData() {
            Object.entries(this.sensors).forEach(sensor => {
                MeasurementsService.getSensorData(sensor[0])
                .then((response) => {
                    this.sensors[sensor[0]] = {
                        id: sensor[0],
                        last: response.data.last,
                        average: response.data.average,
                        color: sensor[1].color
                    };
                })
                .catch((error) => {
                    console.log(error);
                });
            });
        }
    }
}

</script>