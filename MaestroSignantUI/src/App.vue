<template>
  <div>
    <notifications position="top right" group="foo" />

    <router-view />

    <vue-progress-bar />
  </div>
</template>

<script lang="ts">
  import { setupAxios } from './axios-setup';

  export default {
    mounted() {
      setupAxios(this);

      //  [App.vue specific] When App.vue is finish loading finish the progress bar
      this.$Progress.finish();
    },
    created() {
      //  [App.vue specific] When App.vue is first loaded start the progress bar
      this.$Progress.start();

      //  hook the progress bar to start before we move router-view
      this.$router.beforeEach((to, _, next) => {
        //  does the page we want to go to have a meta.progress object
        if (to.meta.progress !== undefined) {
          let meta = to.meta.progress;
          // parse meta tags
          this.$Progress.parseMeta(meta);
        }

        //  start the progress bar
        this.$Progress.start();
        //  continue to next page
        next();
      });

      //  hook the progress bar to finish after we've finished moving router-view
      this.$router.afterEach(() => {
        //  finish the progress bar
        this.$Progress.finish();
      });
    },
  };
</script>

<style lang="scss">
  @import './styles/app.scss';

  body {
    display: block;
    margin: 0;
  }

  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
  }

  #nav {
    padding: 30px;

    a {
      font-weight: bold;
      color: #2c3e50;
    }
  }
</style>
