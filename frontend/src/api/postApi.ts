import {
  ListParams,
  ApiResponse,
  Post,
  PostAdd,
  Comment,
  CommentResponse,
  StatisticParams,
} from 'models';
import CreateApiProvider from './axiosClient';

const axiosClient = CreateApiProvider('https://localhost:7055/api', {
  'content-type': 'multipart/form-data',
});
const axiosClient2 = CreateApiProvider();

const postApi = {
  get(params: ListParams): Promise<ApiResponse<Post>> {
    const url = '/post';
    return axiosClient.get(url, { params });
  },
  getById(id: string): Promise<Post> {
    const url = `/post/${id}`;
    return axiosClient.get(url);
  },
  getForAddEdit(id: string): Promise<PostAdd> {
    const url = `/post/${id}`;
    return axiosClient.get(url);
  },
  delete(id: string): Promise<any> {
    const url = `/post/${id}`;
    return axiosClient.delete(url);
  },
  add(data: FormData): Promise<Post> {
    const url = `/post`;
    return axiosClient.post(url, data);
  },
  update(data: PostAdd, id?: string): Promise<any> {
    const url = `/post/${id}`;
    return axiosClient2.put(url, data);
  },
  postMediaFile(data: FormData, id: string): Promise<any> {
    const url = `/post/${id}/media`;
    return axiosClient.post(url, data);
  },
  createComment(data: Comment, id: string): Promise<any> {
    const url = `/post/${id}/comments`;
    return axiosClient2.post(url, data);
  },
  updateComment(data: Comment, postId: string, commentId: string): Promise<any> {
    const url = `/post/${postId}/comments/${commentId}`;
    return axiosClient2.put(url, data);
  },
  deleteComment(postId: string, commentId: string): Promise<any> {
    const url = `/post/${postId}/comments/${commentId}`;
    return axiosClient2.delete(url);
  },
  getCommentByUserAndPost(id: string): Promise<CommentResponse> {
    const url = `/post/${id}/comment`;
    return axiosClient2.get(url);
  },
  getCommentByPost(id: string, params: ListParams): Promise<ApiResponse<CommentResponse>> {
    const url = `/post/${id}/comments`;
    return axiosClient2.get(url, { params });
  },
  getPostStatistic(params: StatisticParams): Promise<number[]> {
    const url = `/post/statistic`;
    return axiosClient2.get(url, { params });
  },
};

export default postApi;
