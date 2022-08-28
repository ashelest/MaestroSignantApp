<template>
  <div>
    <Header />

    <Dashboard />
  </div>
</template>

<script lang="ts">
  import Dashboard from '@/components/Dashboard.vue';
  import Header from '@/components/Header.vue';
  import { Options, Vue } from 'vue-class-component';

  @Options({
    components: {
      Header,
      Dashboard,
    },
  })
  export default class LandingPageComponent extends Vue {}
</script>
