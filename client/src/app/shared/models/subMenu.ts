
export interface ISubMenu {
    id: number;
    menuHeadId: number;
    subMenuName: string;
    subMenuSeq: string;
    url: string;
}
 
export class SubMenu implements ISubMenu {
    id: number=0;
    menuHeadId: number;
    subMenuName: string;
    subMenuSeq: string;
    url: string;
} 