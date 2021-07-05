import { IEmployeeInfo } from './employeeInfo';

export interface IEmployeePagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmployeeInfo[];
}

export class EmployeePagination implements IEmployeePagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmployeeInfo[] = [];
}