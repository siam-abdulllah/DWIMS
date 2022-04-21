import { ICampaignDtl } from "./campaign";
import { IDonation } from "./donation";
import { IEmployee } from "./employee";
import { IMarketGroupMst } from "./marketGroupMst";
import { IMedicineProduct } from "./medicineProduct";
import { IProduct } from "./product";

export interface IBudgetYearly {
    id: number;
    EmployeeName:string;
   
}
 
export class BudgetYearly implements IBudgetYearly {
    id: number=0;
    EmployeeName:string;
}

