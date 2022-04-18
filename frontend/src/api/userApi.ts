import { ListParams, ApiResponse, Login, JWT, Pagination, User, ChangePassword } from 'models';
import { StringLiteralType } from 'typescript';
import CreateApiProvider from './axiosClient';

const axiosClient = CreateApiProvider();

const userApi = {
  login(data: Login): Promise<JWT> {
    const url = '/user/login';
    return axiosClient.post(url, data);
  },
  get(params: ListParams): Promise<ApiResponse<User>> {
    const url = '/user';
    return axiosClient.get(url, { params });
  },
  getById(id: string): Promise<User> {
    const url = `/user/${id}`;
    return axiosClient.get(url);
  },
  delete(id: string): Promise<any> {
    const url = `/user/${id}`;
    return axiosClient.delete(url);
  },
  register(data: User) {
    const url = '/user/register';
    return axiosClient.post(url, data);
  },
  edit(data: User, id: string) {
    const url = `/user/${id}`;
    return axiosClient.put(url, data);
  },
  changePassword(data: ChangePassword) {
    const url = `/user/password`;
    return axiosClient.post(url, data);
  },
};

export default userApi;
