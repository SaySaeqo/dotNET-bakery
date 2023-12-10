<template>
  <div class="content">
    <v-card>
      <v-tabs v-model="tab">
        <v-tab value="table">Table</v-tab>
        <v-tab value="charts">Charts</v-tab>
      </v-tabs>
      <v-card-text>
        <v-window v-model="tab">
          <v-window-item value="table">
            <data-table-tab></data-table-tab>
          </v-window-item>
          <v-window-item value="charts">
            <charts-tab></charts-tab>
          </v-window-item>
        </v-window>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import DataTableTab from './tabs/table/DataTableTab';
import ChartsTab from './tabs/chart/ChartsTab';

export default {
  name: 'App',
  components: {
    DataTableTab,
    ChartsTab
  },
  data() {
    return {
      tab: null,
    };
  },
}
</script>

<style>
::-webkit-scrollbar {
  display: none;
}

.content {
  padding: 30px;
  background-color: #212121;
  height: 100vh;
  overflow: auto;
}
</style>