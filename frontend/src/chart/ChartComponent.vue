<template>
    <Line :data="data" :options="options" />
</template>

<script>
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
} from 'chart.js';
import { Line } from 'vue-chartjs';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
);

export default {
    name: 'ChartComponent',
    components: { Line },
    props: {
        data: {
            type: Object,
            required: true,
        },
        title: {
            type: String,
            required: true,
        }
    },
    data() {
        return {
            options: {
                layout: {
                    padding: 10,
                },
                plugins: {
                    title: {
                        text: this.title,
                        display: true,
                    }
                },
            }
        };
    },
}

</script>