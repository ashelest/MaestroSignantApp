import { Action, getModule, Module, Mutation, VuexModule } from 'vuex-module-decorators';
import store from '@/store/index';
import { WS_URL as ApiUrl } from '@/config';
import axios from 'axios';
import {
  AttachmentStatus,
  PersonPostingDto,
  PostingClient,
  PostingStatus,
  PostingStatusResult,
  ServiceResultOfPersonPostingDto,
} from '@/apiclient/apiclient_generated';
import { NotificationService } from '@/services/notification-service';
import FileService from '@/services/file.service';
import { ref } from 'vue';

@Module({ dynamic: true, namespaced: true, store, name: 'PersonSignature' })
export class PersonSignatureModule extends VuexModule {
  private client = new PostingClient(ApiUrl, axios);

  personPostings?: PersonPostingDto[] = [];

  @Action
  async sign(command) {
    const { recipientName, recipientEmail, recipientMessage, file } = command;

    const result: ServiceResultOfPersonPostingDto = await this.client.signPosting(recipientName, recipientEmail, recipientMessage, file);

    if (result.isSuccess) {
      this.addPersonPostings(result.entity);

      NotificationService.NotifySuccess(result.message);
    } else {
      NotificationService.NotifyError(result.error);
    }

    return result;
  }

  @Action
  async getPostingStatus(command) {
    const { postingId, attachmentId } = command;

    const postingStatusResult = await this.client.getPostingStatus(postingId, attachmentId);

    const mutationData = {
      postingStatusResult,
      postingId,
    };

    this.processPostingStatusSync(mutationData);
  }

  @Action
  async downloadAttachment(command) {
    const { postingId, attachmentId } = command;
    const file = await this.client.downloadAttachment(postingId, attachmentId);

    if (file && file.data) {
      FileService.downloadFile(file.data, file.fileName);
    }
  }

  @Action
  async getAllPersonPostings() {
    const postings = await this.client.getAllPersonPostings();

    this.setPersonPostings(postings);
  }

  @Mutation
  setPersonPostings(postings: PersonPostingDto[]) {
    this.personPostings = postings;
  }

  @Mutation
  processPostingStatusSync(mutationData) {
    const { postingStatusResult, postingId } = mutationData;

    if (postingStatusResult.status === PostingStatus.Completed) {
      const posting = this.personPostings.find((p) => p.postingId === postingId);
      posting.attachmentStatus = AttachmentStatus.Signed;

      NotificationService.NotifySuccess(postingStatusResult.status);
    } else {
      NotificationService.NotifyWarn(postingStatusResult.status);
    }
  }

  @Mutation
  addPersonPostings(posting: PersonPostingDto) {
    if (!this.personPostings) {
      this.personPostings = [];
    }

    this.personPostings.push(posting);
  }

  @Mutation
  processPostingAttachment(data) {
    const { postingId, status } = data;
    const posting: any = this.personPostings.find((p) => p.postingId.toLowerCase() === postingId.toLowerCase());

    if (!posting) {
      return;
    }

    posting.attachmentStatus = status;
    posting.showPush = ref(true);

    switch (status) {
      case AttachmentStatus.Signed:
        NotificationService.NotifySuccess(`Posting with attachment ${posting.attachmentName} was signed. Now you can download it.`);
        break;

      case AttachmentStatus.Rejected:
        NotificationService.NotifyWarn(`Posting with attachment ${posting.attachmentName} was rejected.`);
        break;
    }
  }
}

export const PersonSignatureStore = getModule(PersonSignatureModule);
