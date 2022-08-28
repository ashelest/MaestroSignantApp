<template>
  <div>
    <div class="ms-header">
      <div class="ms-logo-wrap">
        <!-- App Logo -->
        <div class="app-logo">
          <router-link to="/" tag="a" class="icon">
            <img src="../assets/logo.png" />
          </router-link>
        </div>
      </div>
    </div>

    <hr />
  </div>
</template>

<script lang="ts">
  import { Options, Vue } from 'vue-class-component';

  @Options({})
  export default class Header extends Vue {}
</script>

<style scoped lang="scss">
  @import '@/styles/_variables.scss';

  .ms {
    &-header {
      min-height: 70px;
      background-color: $primary-color;

      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 4px 40px 0;

      .icon {
        img {
          height: 80px;
          margin: 15px 20px;
        }
      }

      .app {
        &-logo {
          display: flex;
          align-content: center;
        }

        &-name {
          color: white;
          font-size: 16px;
          margin-left: 3em;
        }
      }
    }

    &-logo-wrap {
      display: flex;
      justify-content: flex-start;
      align-items: center;
    }
  }

  hr {
    margin: 0 !important;
  }
</style>
