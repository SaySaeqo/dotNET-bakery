export default {
    build(params) {
        let body = {};

        if(params.sortBy && params.sortBy.length) {
            body.sortBy = params.sortBy[0].key;
        }
        if(params.filters.fromDateTime) {
            body.fromDateTime = params.filters.fromDateTime;
        }
        if(params.filters.toDateTime) {
            body.toDateTime = params.filters.toDateTime;
        }
        if(params.filters.types.length) {
            body.types = params.filters.types;
        }
        if(params.filters.ids.length) {
            body.ids = params.filters.ids;
        }

        return body;
    }
}