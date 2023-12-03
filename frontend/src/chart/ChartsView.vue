<template>
    <div>
        <v-row>
            <v-col>
                <ChartComponent :data="temperatureData" title="Temperature" />
            </v-col>
            <v-col>
                <ChartComponent :data="humidityData" title="Humidity" />
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <ChartComponent :data="massData" title="Mass" />
            </v-col>
            <v-col>
                <ChartComponent :data="gasData" title="Gas concentration" />
            </v-col>
        </v-row>       
    </div>
</template>

<script>
import ChartComponent from './ChartComponent.vue';

export default {
    name: 'ChartsView',
    components: { ChartComponent },
    data() {
        return {
            temperatureData: {
                labels: [],
                datasets: [
                    {
                        data: [],
                    },
                ],
            },
            humidityData: {
                labels: [],
                datasets: [
                    {
                        data: [],
                    },
                ],
            },
            massData: {
                labels: [],
                datasets: [
                    {
                        data: [],
                    },
                ],
            },
            gasData: {
                labels: [],
                datasets: [
                    {
                        data: [],
                    },
                ],
            },
        };
    }
}
</script>