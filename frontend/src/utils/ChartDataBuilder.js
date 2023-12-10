import config from '../../config';

const colors = ['red', 'blue', 'green', 'black'];

export default {
    build(data) {
        const step = Math.floor(data.length / 100) || 1;
        let datasets = {};
        data.forEach((element, index) => {
            if(!(index % step)) {
                const point = {
                    x: element.date,
                    y: element.value
                };
                if(!datasets[element.id]) {
                    datasets[element.id] = {
                        data: [point],
                        borderColor: colors[Math.floor((element.id - 1) / config.sensorsPerTypeNumber)]
                    };
                }
                else {
                    datasets[element.id].data.push(point);
                }
            }
        });

        return datasets;
    }
}