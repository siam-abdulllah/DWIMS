import { IClusterMstInfo,IClusterDtlInfo } from './clusterInfo';
export interface IClusterMstPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IClusterMstInfo[];
}

export class ClusterMstPagination implements IClusterMstPagination {
    pageIndex!: number;
    pageSize!: number;
    count!: number;
    data: IClusterMstInfo[] = [];
}
export interface IClusterDtlPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IClusterDtlInfo[];
}

export class ClusterDtlPagination implements IClusterDtlPagination {
    pageIndex!: number;
    pageSize!: number;
    count!: number;
    data: IClusterDtlInfo[] = [];
}