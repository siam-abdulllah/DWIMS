
export interface ISubMenu {
    id: number;
    menuHeadId: number;
    menuHeadName: string;
    menuHeadSeq: string;
    url: string;
}
 
export class SubMenu implements ISubMenu {
    id: number=0;
    menuHeadId: number;
    menuHeadName: string;
    menuHeadSeq: string;
    url: string;
} 