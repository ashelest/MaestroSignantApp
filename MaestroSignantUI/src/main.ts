import App from './App.vue';
import { createApp } from 'vue';
import axios from 'axios';
import VueAxios from 'vue-axios';
import router from './router';
import './utils/polyfills';
import { VueSignalR } from '@dreamonkey/vue-signalr';
import signantHub from '@/signalR/signant-hub';

const app = createApp(App);

const connection = signantHub.init();
app.use(VueSignalR, { connection });

import VueProgressBar from '@aacassandra/vue3-progressbar';
function setupProgressBar() {
  const options = {
    color: '#e3ac19',
    failedColor: '#874b4b',
    thickness: '3px',
    transition: {
      speed: '0.2s',
      opacity: '0.6s',
      termination: 300,
    },
    autoRevert: true,
    location: 'top',
    inverse: false,
  };

  app.use(VueProgressBar, options);
}
setupProgressBar();

app.use(VueAxios, axios);

app.use(router);

import PrimeVue from 'primevue/config';
app.use(PrimeVue);

import Dialog from 'primevue/dialog';
app.component('Dialog', Dialog);
import Button from 'primevue/button';
app.component('Button', Button);
import InputText from 'primevue/inputtext';
app.component('InputText', InputText);
import FileUpload from 'primevue/fileupload';
app.component('FileUpload', FileUpload);

import Tooltip from 'primevue/tooltip';
app.directive('tooltip', Tooltip);

import Notifications from '@kyvg/vue3-notification';
app.use(Notifications);

import BootstrapVue3 from 'bootstrap-vue-3';
app.use(BootstrapVue3);

app.mount('#app');

export default app;
