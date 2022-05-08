import { NumberValueAccessor } from "@angular/forms";
import { ICampaignDtl } from "./campaign";
import { IDonation } from "./donation";
import { IEmployee } from "./employee";
import { IMarketGroupMst } from "./marketGroupMst";
import { IMedicineProduct } from "./medicineProduct";
import { IProduct } from "./product";

export interface IBudgetEmpSbuMap {
    id: number;
    employeeId:number;
    deptId:number;
    compId:number;
    employeeName:string;
    sbuName:string;
    sbu:string;
}
 
export class BudgetEmpSbuMap implements IBudgetEmpSbuMap {
    
    id: number=0;
    employeeId:number;
    employeeName:string;
    sbuName:string;
    deptId:number=0;
    compId:number=1000;
    sbu:string=null;
}


 




