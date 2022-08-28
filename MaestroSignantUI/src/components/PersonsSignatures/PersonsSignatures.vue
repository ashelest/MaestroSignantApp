<template>
  <div>
    <div class="signatures-header">
      <h2>Posting Signatures</h2>
    </div>

    <div class="person-signatures ms-scroll">
      <div class="col-sm-12 col-md-12 col-lg-12" v-for="(posting, index) in personPostings" :key="index">
        <PersonSignature :posting="posting" />
      </div>
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
      return postings.sort((a, b) => a.createdDate - b.createdDate);
    }
  }
</script>

<style scoped lang="scss">
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
