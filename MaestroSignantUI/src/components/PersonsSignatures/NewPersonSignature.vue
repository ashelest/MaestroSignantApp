<template>
  <div>
    <div class="new-signature-btn" v-if="!showWizard">
      <button type="button" class="add-btn" @click="showWizard = true">Sign</button>
    </div>

    <Dialog
      header="Sign document"
      :visible="showWizard"
      :closable="false"
      v-model="showWizard"
      :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
      :style="{ width: '50vw' }"
      :modal="true">
      <div class="m-0 modal-body-container">
        <div class="person-info">
          <div class="person-info-label">Recipient Name</div>
          <InputText type="text" class="form-control" v-model="postingSignInfo.recipientName" placeholder="Recipient Name" />
        </div>

        <div class="person-info">
          <div class="person-info-label">Recipient Email</div>
          <InputText
            type="email"
            class="form-control"
            :class="{ 'p-invalid': postingSignInfo.recipientEmail && !validEmail(postingSignInfo.recipientEmail) }"
            v-model="postingSignInfo.recipientEmail"
            placeholder="Recipient Email" />
          <div v-if="!isRecipientEmailValid" class="person-info-error-label">Recipient Email</div>
        </div>

        <div class="person-info">
          <div class="person-info-label">Message to the recipient</div>
          <InputText type="text" class="form-control" v-model="postingSignInfo.recipientMessage" placeholder="Message to the recipient" />
        </div>

        <div class="person-info">
          <div class="person-info-label">Document to Sign</div>
          <input
            type="file"
            class="form-control"
            ref="docRef"
            v-on:change="processFileLoad($event)"
            accept="application/pdf"
            placeholder="Select any pfd file" />
        </div>

        <div class="loading-spinner" v-if="isLoading">
          <ProgressSpinner />
        </div>
      </div>
      <template #footer>
        <Button label="Cancel" icon="pi pi-times" :disabled="isLoading" @click="cancel" class="p-button-danger" />
        <Button label="Sign" icon="pi pi-check" :disabled="!isFormValid" @click="sign" class="p-button-warning" autofocus />
      </template>
    </Dialog>
  </div>
</template>

<script lang="ts">
  import { Options, Vue } from 'vue-class-component';
  import PostingSignInfo from '@/types/posting-sign-info';
  import { PersonSignatureModule, PersonSignatureStore } from '@/store/personSignature';
  import ProgressSpinner from 'primevue/progressspinner';
  @Options({ components: { ProgressSpinner } })
  export default class NewPersonSignature extends Vue {
    private store: PersonSignatureModule = PersonSignatureStore;

    isLoading = false;
    fileSelected = null;
    postingSignInfo = {} as PostingSignInfo;
    showWizard = false;

    cancel() {
      this.fileSelected = false;
      this.postingSignInfo = {} as PostingSignInfo;
      this.showWizard = false;
    }

    async sign() {
      this.isLoading = true;
      const formData = this.getFormData();

      const result = await this.store.sign(formData);

      this.isLoading = false;
      if (result.isSuccess) {
        this.cancel();
      }
    }

    private getFormData() {
      return {
        file: this.getFile(),

        recipientName: this.postingSignInfo.recipientName,
        recipientEmail: this.postingSignInfo.recipientEmail,
        recipientMessage: this.postingSignInfo.recipientMessage,
      };
    }

    private getFile() {
      const fileRef: any = this.fileRef;

      const file = fileRef.files[0];

      return {
        fileName: file.name,
        data: file,
      };
    }

    get fileRef() {
      return this.$refs.docRef;
    }

    processFileLoad($event) {
      this.fileSelected = $event;
    }

    validEmail(email) {
      var re =
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

      return re.test(email);
    }

    get isFormValid() {
      return (
        !this.isLoading &&
        this.fileSelected &&
        this.postingSignInfo.recipientName &&
        this.postingSignInfo.recipientEmail &&
        this.validEmail(this.postingSignInfo.recipientEmail) &&
        this.postingSignInfo.recipientMessage
      );
    }

    get isRecipientEmailValid() {
      return true;
    }
  }
</script>

<style scoped lang="scss">
  @import '@/styles/_variables.scss';

  .new-signature-btn {
    position: fixed;
    bottom: 18px;
    right: 18px;

    .add-btn {
      background: $secondary-color;
      height: 80px;
      width: 80px;
      border-radius: 50%;
      box-shadow: 2px 3px 2px rgb(0 0 0 / 10%);

      font-size: 2rem;
    }
  }

  .btn {
    display: inline-block;
    justify-content: flex-start;
    align-items: center;

    align-content: center;
    width: 7em;
    background-color: $primary-color;
    font-size: 20px;
    color: white;
    border: 1px solid $primary-color;
    border-radius: 8px;

    // :first-child {
    //   padding-right: 20px;
    // }

    .icon {
      font-size: 20px;
    }

    &:active {
      background-color: #385e85;
    }

    &:hover {
      cursor: pointer;

      background-color: white;

      .pi {
        color: $primary-color;
      }
    }
  }

  .btn-cancel {
    width: 3em;
    .pi {
      color: $danger-color;
    }
    background-color: white;

    &:hover {
      cursor: pointer;
    }
  }

  .person-info {
    margin-bottom: 15px;

    &-label {
      font-size: 16px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      font-size: 1rem;
    }

    &-error-label {
      font-size: 1rem;
      padding-top: 8px;
      color: red;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
  }

  .modal-body-container {
    position: relative;
    .loading-spinner {
      position: absolute;
      top: 40%;
      left: 42%;
    }
  }
</style>
