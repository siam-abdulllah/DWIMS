export interface ISocietyInfo {
    id: number;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    status: string;
    setOn: Date;
}

export class SocietyInfo implements ISocietyInfo {
    id: number = 0;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    status: string;
    setOn: Date;
}