<template>
    <div class="pa-8">
        <v-data-table-server
            v-model:items-per-page="itemsPerPage"
            :headers="headers"
            :items-length="totalItems"
            :items="measurements"
            :loading="loading"
            :search="search"
            item-value="id"
            @update:options="loadItems"
            theme="dark"
            class="rounded-lg"
            :items-per-page-options="[10, 25, 50]"
        >
            <template v-slot:top>
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
                            v-model="filters.types"
                            :items="[ 'gas_density', 'humidity', 'temperature', 'weight' ]"
                            multiple
                            label="Types"
                            chips
                        />  
                        </v-col>
                        <v-col>
                            <v-select 
                            v-model="filters.ids"
                            :items="idsItems"
                            multiple
                            label="IDs"
                            />
                        </v-col>
                        </v-row>
                        <v-row>
                        <v-col>
                            <v-btn 
                            class="ma-2" 
                            @click="this.search = String(Date.now())"
                            >
                            Search
                            </v-btn>
                            <v-btn 
                            class="ma-2" 
                            @click="clearFilters()"
                            >
                            Clear
                            </v-btn>
                        </v-col>
                        <v-col>
                            <v-btn 
                            class="ma-2" 
                            @click="downloadFile('json')"
                            prepend-icon="mdi-download"  
                            >
                            Download JSON file
                            </v-btn>
                            <v-btn 
                            class="ma-2" 
                            @click="downloadFile('csv')"
                            prepend-icon="mdi-download"  
                            >
                            Download CSV file
                            </v-btn>
                        </v-col>
                        </v-row>
                    </v-container>           
                    </v-form>
                </v-expansion-panel-text>
                </v-expansion-panel>
            </v-expansion-panels>
            </template>
        </v-data-table-server>
    </div>
</template>
  
<script>
import MeasurementsService from '../../service/MeasurementsService';
import dateParser from '../../utils/DateParser';
import requestBodyBuilder from '../../utils/RequestBodyBuilder';
import config from '../../../config';
  
export default {
    name: 'DataTableTab',
    data() {
      return {
        itemsPerPage: 10,
        headers: [
          { title: 'Sensor ID', key: 'id', sortable: true },
          { title: 'Date', key: 'date', sortable: true },
          { title: 'Value', key: 'value', sortable: true },
          { title: 'Type', key: 'type', sortable: true },
        ],
        totalItems: undefined,
        measurements: [],
        loading: false,
        search: '',
        filters: {
          fromDateTime: null,
          toDateTime: null,
          types: [],
          ids: [],
        },
        idsItems: Array.from({ length: config.measurementTypesNumber * config.sensorsPerTypeNumber }, (_, i) => i + 1),
        sortBy: null,
        showCharts: false,
      };
    },
    methods: {
      loadItems({ itemsPerPage, page, sortBy }) {
        this.sortBy = sortBy;
        this.loading = true;
        const filters = this.filters;
        MeasurementsService.get(requestBodyBuilder.build({ sortBy, filters }))
            .then((response) => {
              this.measurements = response.data
                .slice((page - 1) * itemsPerPage, page * itemsPerPage)
                .map(measurement => {
                return {
                  id: measurement.id,
                  date: dateParser.parse(measurement.date),
                  value: measurement.value,
                  type: measurement.type
                };
              });
              this.totalItems = response.data.length;
              this.loading = false;
            })
            .catch((error) => {
              console.log(error);
            });
      },
      clearFilters() {
        this.filters.fromDateTime = null;
        this.filters.toDateTime = null;
        this.filters.ids = [];
        this.filters.types = [];
      },
      downloadFile(type) {
        const sortBy = this.sortBy;
        const filters = this.filters;
        MeasurementsService.getFile(type, requestBodyBuilder.build({ sortBy, filters }))
          .then((response => {
            try {
              const blob = new Blob([response.data], { type: response.headers['Content-type'] });
              const link = document.createElement('a');
              link.href = window.URL.createObjectURL(blob);
              link.download = `measurements.${type}`
              link.click();
  
              window.URL.revokeObjectURL(link.href); 
            }
            catch(error) {
              console.log(error);
            }
          }))
          .catch(error => {
            console.log(error);
          });
      }
    },
}
</script>
