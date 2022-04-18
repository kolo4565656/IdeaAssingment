import { ListParams, ApiResponse, Category } from 'models';
import CreateApiProvider from './axiosClient';

const axiosClient = CreateApiProvider();

const categoryApi = {
  get(params: ListParams): Promise<ApiResponse<Category>> {
    const url = '/category';
    return axiosClient.get(url, { params });
  },
  add(data: Category[]): Promise<any> {
    const url = '/category';
    return axiosClient.post(url, data);
  },
};

export default categoryApi;
