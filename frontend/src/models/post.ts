import { Category } from './category';
export interface Post {
  id: string;
  name: string;
  created: Date;
  createdBy: string;
  creatorName: string;
  modifierName: string;
  description: string;
  commentCount: number;
  lastModified: Date;
  modifiedBy: string;
  totalStar: number;
  dislikeCount?: number;
  likeCount?: number;
  content: string;
  isAttachs?: boolean;
  categories: Category[];
}

export interface PostAdd {
  name?: string;
  content?: string;
  description?: string;
  categoryIds?: string | number[];
  postImage?: string;
}

export interface Comment {
  name?: string;
  content?: string;
  rating?: 0;
}

export interface CommentResponse {
  id?: string;
  userId?: string;
  name?: string;
  content?: string;
  rating?: number;
  createdDate?: Date;
  updatedDate?: Date;
  userFullName?: string;
  subComments?: SubComment;
}

export interface SubComment {
  id?: string;
  commentId?: string;
  userId?: string;
  content?: string;
  updatedDate?: Date;
  userFullName?: string;
}
