import { NumberValueAccessor } from "@angular/forms";
import { ICampaignDtl } from "./campaign";
import { IDonation } from "./donation";
import { IEmployee } from "./employee";
import { IMarketGroupMst } from "./marketGroupMst";
import { IMedicineProduct } from "./medicineProduct";
import { IProduct } from "./product";

export interface IBudgetYearly {
    id: number;
    compId:number;
    deptId:number;
    year:number;
    totalAmount:number;
    remarks:string;
    enteredBy:number;
   
}
 
export class BudgetYearly implements IBudgetYearly {
    id: number=0;
    compId:number=0;
    deptId:number=0;
    year:number;
    totalAmount:number;
    remarks:string;
    enteredBy:number;

}

