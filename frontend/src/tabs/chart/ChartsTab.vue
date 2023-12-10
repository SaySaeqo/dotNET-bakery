<template>
    <v-card>
        <v-tabs v-model="tab">
            <v-tab value="gas_density">Gas density</v-tab>
            <v-tab value="humidity">Humidity</v-tab>
            <v-tab value="temperature">Temperature</v-tab>
            <v-tab value="weight">Weight</v-tab>
        </v-tabs>
        <v-card-text>
            <v-window v-model="tab">
                <v-window-item value="gas_density">
                  <ChartComponent title="Gas density chart" type="gas_density" />
                </v-window-item>
                <v-window-item value="humidity">
                  <ChartComponent title="Humidity chart" type="humidity" />
                </v-window-item>
                <v-window-item value="temperature">
                  <ChartComponent title="Temperature chart" type="temperature" />
                </v-window-item>
                <v-window-item value="weight">
                  <ChartComponent title="Weight chart" type="weight" />
                </v-window-item>
            </v-window>
        </v-card-text>
    </v-card>
</template>

<script>
import ChartComponent from './ChartComponent.vue';

export default {
    name: 'ChartsView',
    components: { ChartComponent },
    data() {
        return {
            tab: null
        };
    },
}
</script>