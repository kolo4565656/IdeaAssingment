import axios, { AxiosRequestConfig, AxiosRequestHeaders, AxiosResponse } from 'axios';

export default function CreateApiProvider(
  baseURL: string = 'https://localhost:7055/api',
  headers: AxiosRequestHeaders = {
    'Content-Type': 'application/json',
  }
) {
  console.log('call');
  const token: string | null = localStorage.getItem('access_token');
  if (token) {
    headers = { ...headers, Authorization: `bearer ${token}` };
  }
  const axiosClient = axios.create({
    baseURL: baseURL,
    headers: headers,
  });

  // Add a request interceptor
  axiosClient.interceptors.request.use(
    function (config: AxiosRequestConfig) {
      // Do something before request is sent
      return config;
    },
    function (error) {
      // Do something with request error
      console.log(error);
      return Promise.reject(error);
    }
  );

  // Add a response interceptor
  axiosClient.interceptors.response.use(
    function (response: AxiosResponse) {
      // Any status code that lie within the range of 2xx cause this function to trigger
      // Do something with response data
      return response.data;
    },
    function (error) {
      // Any status codes that falls outside the range of 2xx cause this function to trigger
      // Do something with response error
      return Promise.reject(error);
    }
  );
  return axiosClient;
}
