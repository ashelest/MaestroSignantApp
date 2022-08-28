<template>
  <div class="p-signature">
    <div class="p-signature-name p-signature-receiver">
      <div class="p-signature-name-text">To: {{ posting.personName }}</div>
    </div>

    <div class="p-signature-name">
      <div class="p-signature-name-text">{{ posting.attachmentName }}</div>
    </div>

    <div class="p-signature-name">
      <div
        class="p-signature-name-status"
        :class="{
          'p-signature-status-new': isPostingCreated,
          'p-signature-status-signed': isPostingSigned,
          'p-signature-status-rejected': isPostingRejected,
        }">
        {{ posting.attachmentStatus }}
      </div>

      <div class="flex posting-action-icons">
        <i
          @click="handleRefreshClick()"
          :class="{
            'posting-action-icons-active': isPostingCreated,
            'posting-action-icons-inactive': isPostingSigned,
            'posting-action-icons-rejected': isPostingRejected,
          }"
          class="pi pi-refresh p-signature-icon cursor-pointer"></i>
        <i
          @click="handleDownloadClick()"
          :class="{
            'posting-action-icons-active': isPostingSigned,
            'posting-action-icons-inactive': isPostingCreated,
            'posting-action-icons-rejected': isPostingRejected,
          }"
          class="pi pi-download p-signature-icon cursor-pointer"></i>
      </div>
    </div>

    <div class="p-signature-push" v-if="posting.showPush">
      <div class="push-circle" v-tooltip="{ value: getPostingTooltipText }">
        <div>1</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { AttachmentStatus, PersonPostingDto } from '@/apiclient/apiclient_generated';
  import { PersonSignatureModule, PersonSignatureStore } from '@/store/personSignature';
  import { Options, Vue } from 'vue-class-component';
  import { Prop } from 'vue-property-decorator';

  @Options({})
  export default class PersonSignature extends Vue {
    private store: PersonSignatureModule = PersonSignatureStore;

    @Prop() posting: PersonPostingDto;

    get isPostingRejected() {
      return this.posting.attachmentStatus === AttachmentStatus.Rejected;
    }

    get isPostingCreated() {
      return this.posting.attachmentStatus === AttachmentStatus.Created;
    }

    get isPostingSigned() {
      return this.posting.attachmentStatus === AttachmentStatus.Signed;
    }

    get getPostingTooltipText() {
      if (this.isPostingRejected) {
        return 'This posting was rejected.';
      }

      return 'Could be downloaded.';
    }

    async handleRefreshClick() {
      if (this.isPostingCreated) {
        const command = {
          postingId: this.posting.postingId,
          attachmentId: this.posting.attachmentId,
        };

        await this.store.getPostingStatus(command);
      }
    }

    async handleDownloadClick() {
      if (this.isPostingSigned) {
        const posting: any = this.posting;
        posting.showPush = false;

        const command = {
          postingId: this.posting.postingId,
          attachmentId: this.posting.attachmentId,
        };

        await this.store.downloadAttachment(command);
      }
    }
  }
</script>

<style scoped lang="scss">
  @import '@/styles/_variables.scss';

  .p-signature {
    display: grid;
    grid-template-columns: 25% 55% 15%;
    align-items: center;
    column-gap: 20px;
    box-shadow: 2px 3px 2px rgb(0 0 0 / 12%);
    border: 0.1em solid #e6e6e6;
    padding: 15px 15px;
    margin-bottom: 10px;
    border-radius: 4px;
    position: relative;

    &-push {
      position: absolute;
      right: -10px;
      top: 0;

      .push-circle {
        background: $secondary-color;
        width: 30px;
        height: 30px;
        border-radius: 50%;

        display: flex;
        justify-content: center;
        align-items: center;

        div {
          color: white;
          font-size: 1.2rem;
          font-weight: 700;
        }
      }
    }

    &-receiver {
      padding-left: 10px;
    }

    .posting-action-icons {
      display: inline-flex;
      padding: 5px 10px;

      &-active {
        -webkit-text-stroke: 1px black;
      }

      &-inactive {
        -webkit-text-stroke: 1px gray;
        cursor: not-allowed !important;
      }
      &-rejected {
        -webkit-text-stroke: 1px gray;
        cursor: not-allowed !important;
      }
    }

    &-icon {
      font-size: 1.5em;
      border-radius: 70%;
      padding: 5px 15px;
    }

    .p-signature-status-new {
      border: 2px solid $secondary-color;
      border-radius: 5px;
      min-width: 80px;
    }
    .p-signature-status-signed {
      border: 2px solid $success-color;
      border-radius: 5px;
      min-width: 80px;
    }
    .p-signature-status-rejected {
      border: 2px solid $danger-color;
      border-radius: 5px;
      min-width: 80px;
    }

    &-name {
      display: flex;
      justify-content: flex-start;
      align-items: center;

      &-status {
        font-size: 16px;
        flex: 0 1 auto;
        text-overflow: ellipsis;
        font-size: 1.2rem;
        padding: 0 5px;
      }

      &-text {
        font-size: 16px;
        flex: 0 1 auto;
        padding-right: 5px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        font-size: 1.2rem;
      }
    }
  }
</style>
