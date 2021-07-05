import { IDonation } from './donation';
export interface IDonationPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IDonation[];
}

export class DonationPagination implements IDonationPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IDonation[] = [];
}