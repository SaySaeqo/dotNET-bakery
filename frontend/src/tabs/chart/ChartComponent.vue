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
        <v-row class="my-16">
            <v-expansion-panels style="width: 100%;">
                <v-expansion-panel title="Filters">
                    <v-expansion-panel-text>
                        <v-form>
                            <v-container>
                                <v-row>
                                    <v-col>
                                        <v-text-field
                                            v-model="filters.fromDateTime"
                                            label="From date"
                                            hint="yyyy-mm-dd hh:mm:ss"
                                        />
                                    </v-col>
                                    <v-col>
                                        <v-text-field 
                                            v-model="filters.toDateTime"
                                            label="To date"
                                            hint="yyyy-mm-dd hh:mm:ss"
                                        />
                                    </v-col>
                                    <v-col>
                                        <v-select 
                                            v-model="filters.ids"
                                            :items="Object.keys(this.sensors)"
                                            multiple
                                            label="IDs"
                                        />
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col>
                                        <v-btn 
                                            class="ma-2" 
                                            @click="getData()"
                                        >
                                            Apply
                                        </v-btn>
                                        <v-btn 
                                            class="ma-2" 
                                            @click="clearFilters()"
                                        >
                                            Clear
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-container>           
                        </v-form>
                    </v-expansion-panel-text>
                </v-expansion-panel>
            </v-expansion-panels>
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
import RequestBodyBuilder from '@/utils/RequestBodyBuilder';

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
                        title: {
                            display: true,
                            text: 'Date'
                        },
                        ticks: {
                            source: 'auto'
                        }
                    },
                    y: {
                        title: {
                            display: true,                          
                        }
                    }
                },
            },
            filters: {
                fromDateTime: null,
                toDateTime: null,
                ids: [],
            },
        };
    },
    async created() {
        this.filters.types = [this.type];
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
            const filters = this.filters;
            const sortBy = [{ key: 'date' }];
            MeasurementsService.get(RequestBodyBuilder.build({ sortBy, filters }))
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
        },
        clearFilters() {
            this.filters.fromDateTime = null;
            this.filters.toDateTime = null;
            this.filters.ids = [];
        },
    }
}

</script>