import { ICampaignMst,ICampaignDtl,ICampaignDtlProduct } from './campaign';
export interface ICampaignMstPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignMst[];
}

export class CampaignMstPagination implements ICampaignMstPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignMst[] = [];
}
export interface ICampaignDtlPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignDtl[];
}

export class CampaignDtlPagination implements ICampaignDtlPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignDtl[] = [];
}
export interface ICampaignDtlProductPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignDtlProduct[];
}

export class CampaignDtlProductPagination implements ICampaignDtlProductPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICampaignDtlProduct[] = [];
}