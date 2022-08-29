<template>
  <div>
    <div class="signatures-header">
      <div class="signatures-header-text">
        <h2>Posting Signatures</h2>
      </div>
    </div>

    <div v-if="personPostings.length" class="person-signatures ms-scroll">
      <div class="col-sm-12 col-md-12 col-lg-12" v-for="(posting, index) in personPostings" :key="index">
        <PersonSignature :posting="posting" />
      </div>
    </div>

    <div v-if="!personPostings.length" class="no-content">
      <div class="header">No Content</div>
      <div class="body">Please, select document to Sign</div>
    </div>

    <NewPersonSignature />
  </div>
</template>

<script lang="ts">
  import { PersonSignatureModule, PersonSignatureStore } from '@/store/personSignature';
  import { Options, Vue } from 'vue-class-component';
  import NewPersonSignature from './NewPersonSignature.vue';
  import PersonSignature from './PersonSignature.vue';

  @Options({
    components: { PersonSignature, NewPersonSignature },
  })
  export default class PersonsSignatures extends Vue {
    private store: PersonSignatureModule = PersonSignatureStore;

    async mounted() {
      await this.store.getAllPersonPostings();
    }

    get personPostings() {
      const postings: any = this.store.personPostings;
      return postings.sort(p => p.createdDate);
    }
  }
</script>

<style scoped lang="scss">
  @import '@/styles/_variables.scss';

  .no-content {
    margin-top: 5%;
    border: 4px solid #e3ac19;
    flex-direction: column;
    display: inline-flex;
    padding: 120px;
    border-radius: 10px;

    .header {
      font-size: 38px;
    }

    .body {
      font-size: 28px;
    }
  }

  .signatures-header-text {
    widows: 400px;
  }

  .person-signatures {
    height: 70vh;
    overflow: hidden;
    overflow-y: auto;
    padding: 0 10px;
  }

  .signatures-header {
    padding: 10px;
  }
</style>
