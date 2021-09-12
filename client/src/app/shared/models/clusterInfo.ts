export interface IClusterMstInfo {
    id: number;
    clusterCode : string;
    clusterName : string;
    status:string;
}

export class ClusterMstInfo implements IClusterMstInfo {
    id: number=0;
    clusterCode : string;
    clusterName : string;
    status:string=null;
}
export interface IClusterDtlInfo {
    id: number;
    mstId: number;
    regionCode : string;
    regionName : string;
    status:string;
}

export class ClusterDtlInfo implements IClusterDtlInfo {
    id: number=0;
    mstId: number;
    regionCode : string;
    regionName : string;
    status:string=null;
}