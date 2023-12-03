<template>
  <div class="content">
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
    />
    <charts-view></charts-view>
  </div>
</template>

<script>
import ChartsView from './chart/ChartsView';

export default {
  name: 'App',
  components: { ChartsView },
  data() {
    return {
      itemsPerPage: 10,
      headers: [
        { title: 'Id', key: 'id', sortable: false },
        { title: 'Date', key: 'date', sortable: true },
        { title: 'Time', key: 'time', sortable: true },
        { title: 'Critical', key: 'isCritical', sortable: false },
        { title: 'Value', key: 'value', sortable: true },
        { title: 'Type', key: 'type', sortable: false },
      ],
      totalItems: 100,
      measurements: [ { id: 1, date: "03-12-2023", time: "21:11", isCritical: false, value: 100.0, type: "Temperature" } ],
      loading: false,
      search: '',
    };
  },
  methods: {
    loadItems() {
      this.loading = true;
      // API call
      // this.measurements = measurements;
      // this.totalItems = total;
      this.loading = false;
    }
  },
}
</script>

<style>
::-webkit-scrollbar {
  display: none;
}

.content {
  padding: 30px;
  background-color: #424242;
  height: 100vh;
  overflow: auto;
}
</style>