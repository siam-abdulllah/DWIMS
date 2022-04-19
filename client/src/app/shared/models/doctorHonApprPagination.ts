import { IDoctorHonAppr } from './doctorHonAppr';

export interface IDoctorHonApprPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IDoctorHonAppr[];
}

export class DoctorHonApprPagination implements IDoctorHonApprPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IDoctorHonAppr[] = [];
}