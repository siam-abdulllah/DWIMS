import { IUserResponse } from './user';
export interface IUserPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IUserResponse[];
}

export class UserPagination implements IUserPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IUserResponse[] = [];
}