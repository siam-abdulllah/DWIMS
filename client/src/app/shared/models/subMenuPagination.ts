import { ISubMenu } from './subMenu';

export interface ISubMenuPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISubMenu[];
}

export class SubMenuPagination implements ISubMenuPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISubMenu[] = [];
}