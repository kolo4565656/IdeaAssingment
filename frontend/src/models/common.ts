import { Role } from 'models';
export interface Pagination {
  pageIndex?: number;
  pageSize: number;
  totalItemsCount?: number;
  totalPages: number;
  totalDuration?: number;
  queryDuration?: number;
  countDuration?: number;
  buildingQueryDuration?: number;
}

export interface FieldInfo {
  field: string;
  value: any;
}

export interface ApiResponse<T> {
  data: T[];
  paging: Pagination;
}

export interface ListParams {
  pageIndex?: number;
  pageSize?: number;
  sort?: string;
  keyword?: string;
  [key: string]: any;
}

export interface StatisticParams {
  year?: number;
  category?: number;
}
