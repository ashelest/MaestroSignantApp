import { useSignalR } from '@dreamonkey/vue-signalr';
import { WS_URL } from '@/config';
import { VueSignalR } from '@dreamonkey/vue-signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { PersonSignatureStore } from '@/store/personSignature';
import { AttachmentStatus } from '@/apiclient/apiclient_generated';

const signantHub = {
  init() {
    // const signalr = useSignalR();

    const connection = new HubConnectionBuilder()
      .withUrl(WS_URL + '/SignantHub')
      .withAutomaticReconnect()
      .build();

    connection.on('PostingSigned', (postingId) => {
      const params = { postingId: postingId, status: AttachmentStatus.Signed };
      PersonSignatureStore.processPostingAttachment(params);
    });

    connection.on('PostingRejected', (postingId) => {
      const params = { postingId: postingId, status: AttachmentStatus.Rejected };
      PersonSignatureStore.processPostingAttachment(params);
    });

    return connection;
  },
};

export default signantHub;
