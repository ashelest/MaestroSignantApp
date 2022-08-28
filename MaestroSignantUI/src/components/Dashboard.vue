<template>
  <div class="body-container ms-scroll">
    <PersonsSignatures />
  </div>
</template>

<script lang="ts">
  import { Options, Vue } from 'vue-class-component';
  import PersonsSignatures from './PersonsSignatures/PersonsSignatures.vue';

  @Options({
    components: {
      PersonsSignatures,
    },
  })
  export default class Dashboard extends Vue {}
</script>

<style scoped lang="scss">
  .body-container { }
</style>
