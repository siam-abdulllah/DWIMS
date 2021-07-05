import { ICampaign } from './campaign';
export interface ICampaignPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaign[];
}

export class CampaignPagination implements ICampaignPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaign[] = [];
}