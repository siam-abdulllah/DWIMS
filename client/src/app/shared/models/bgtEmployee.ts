export interface IBgtEmpInsertDto {
    deptId: number;
    year: number;
    sbu: string;
    authId: number;
    amount: number;
    segment: string;
    permEdit: any;
    permView: any;
    enteredBy: number;
}

export class BgtEmpInsertDto implements IBgtEmpInsertDto {
    deptId: number;
    year: number;
    sbu: string;
    authId: number;
    amount: number;
    segment: string;
    permEdit: any;
    permView: any;
    enteredBy: number;
}


export interface IBgtOwnInsertDto {
    deptId: number;
    year: number;
    sbu: string;
    authId: number;
    amount: number;
    amtLimit: number;
    segment: string;
    enteredBy: number;
    donationId: number;
}

export class BgtOwnInsertDto implements IBgtOwnInsertDto {
    deptId: number;
    year: number;
    sbu: string;
    authId: number;
    amount: number;
    amtLimit: number;
    segment: string;
    enteredBy: number;
    donationId: number;
}