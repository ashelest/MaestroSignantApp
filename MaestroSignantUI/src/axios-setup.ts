import axios from 'axios';

export function setupAxios(context) {
  axios.defaults.withCredentials = true;

  axios.interceptors.request.use(
    async (request: any) => {
      context.$Progress.start();

      return request;
    },
    (err) => {
      return Promise.reject(err);
    }
  );

  axios.interceptors.response.use(
    (response: any) => {
      context.$Progress.finish();

      return response;
    },

    async (error) => {
      console.log(error);

      return Promise.reject(error);
    }
  );
}
