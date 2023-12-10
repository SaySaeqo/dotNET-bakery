export default {
    build(params) {
        let body = {};

        if(params.sortBy && params.sortBy.length) {
            body.sortBy = params.sortBy[0].key;
        }
        if(params.filters.fromDateTime) {
            body.fromDateTime = params.filters.fromDateTime.slice(0, 10) + 'T' + params.filters.fromDateTime.slice(11);
        }
        if(params.filters.toDateTime) {
            body.toDateTime = params.filters.toDateTime.slice(0, 10) + 'T' + params.filters.toDateTime.slice(11);
        }
        if(params.filters.types.length) {
            body.types = params.filters.types;
        }
        if(params.filters.ids.length) {
            body.ids = params.filters.ids;
        }

        console.log(body);

        return body;
    }
}