
export interface IMenuHead {
    id: number;
    menuHeadName: string;
    menuHeadSeq: string;
}
 
export class MenuHead implements IMenuHead {
    id: number=0;
    menuHeadName: string;
    menuHeadSeq: string;
} 