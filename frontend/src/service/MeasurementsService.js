import axios from 'axios';
import config from '../../config';

const measurementsService = axios.create({
    baseURL: config.apiBaseUrl,
});

export default {
    get(body) {
        return measurementsService.post('/json', body);
    },
    getCSV(body) {
        return measurementsService.post('/csv', body);
    },
    getFile(type, body) {
        if(type == 'csv') {
            return measurementsService.post('/csv', body, { responseType: 'blob' });
        }

        return measurementsService.post('/json', body, { responseType: 'blob' });
    },
    getSensorData(id) {
        return measurementsService.get(`/json/${id}`);
    }
}