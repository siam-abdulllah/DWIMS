
export interface IInstitution {
    id: number;
    institutionName: string;
    institutionCode: string;
    institutionID: number;
    institutionType: string;
    status: string; 
    setOn: Date;
}
 
export class Institution implements IInstitution {
    id: number=0;
    institutionName: string;
    institutionCode: string;
    institutionID: number;
    institutionType: string;
    setOn: Date;
    status: string="Active"; 
}