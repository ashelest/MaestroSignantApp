import { notify } from '@kyvg/vue3-notification';

export class NotificationService {
  static NotifySuccess(message: any = '', duration = 8000) {
    if (!message) message = 'Success.';

    notify({
      group: 'foo',
      text: message,
      type: 'success',
      duration: duration,
    });
  }

  static NotifyError(message: string, title = 'Error', duration = -1) {
    notify({
      group: 'foo',
      title: title,
      text: message,
      type: 'error',
      duration: duration,
    });
  }

  static NotifyWarn(message: string, duration = 8000) {
    notify({
      group: 'foo',
      text: message,
      type: 'warn',
      duration: duration,
    });
  }
}
