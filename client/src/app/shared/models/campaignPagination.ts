import { ICampaignMst } from './campaign';
export interface ICampaignPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignMst[];
}

export class CampaignPagination implements ICampaignPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignMst[] = [];
}