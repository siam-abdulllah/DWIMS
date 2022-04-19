
export interface IMenuConfig {
    id: number;
    menuHeadId: number;
    menuHeadName: string;
    subMenuId: number;
    subMenuName: string;
    roleId: string;
    roleName: string;
    url: string;
}
 
export class MenuConfig implements IMenuConfig {
    id: number=0;
    menuHeadId: number;
    menuHeadName: string;
    subMenuId: number;
    subMenuName: string;
    roleId: string;
    roleName: string;
    url: string;
} 