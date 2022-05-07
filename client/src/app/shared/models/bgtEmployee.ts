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