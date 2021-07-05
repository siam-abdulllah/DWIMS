import { ISubCampaign } from './subCampaign';
export interface ISubCampaignPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISubCampaign[];
}

export class SubCampaignPagination implements ISubCampaignPagination {
    pageIndex!: number;
    pageSize!: number;
    count!: number;
    data: ISubCampaign[] = [];
}