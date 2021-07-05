export interface IBcdsInfo {
    id: number;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    status: string;
    setOn: Date;
}

export class BcdsInfo implements IBcdsInfo {
    id: number = 0;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    status: string;
    setOn: Date;
}