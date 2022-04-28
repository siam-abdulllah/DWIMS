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

export interface IBudgetSbuYearly {
    id: number;
    compId:number;
    deptId:number;
    year:number;
    sbuCode:string;
    sbuName:string;
    sbuAmount:any;
    remarks:string;
    enteredBy:number;
    totalBudget:number;
    remainingBudget:number;
    expense:number;
    sbuDetailsList:ISbuDetails[];
   
}
 
export class BudgetSbuYearly implements IBudgetSbuYearly {
    id: number=0;
    compId:number=100;
    deptId:number=0;
    year:number;
    sbuCode:string=null;
    sbuName:string;
    sbuAmount:number;
    remarks:string;
    enteredBy:number;
    totalBudget:any;
    remainingBudget:number;
    expense:number;
    sbuDetailsList:ISbuDetails[];

}

export interface ISbuDetails {
    sbuName:string;
    sbuCode:string;
    sbuAmount:number;
    newAmount:any;
    expense:number;
    pipeLine:any;
    remaining:any;
}
export class SbuDetails implements ISbuDetails {
    sbuName:string;
    sbuCode:string;
    sbuAmount:number;
    newAmount:any;
    expense:number;
    pipeLine:any;
    remaining:any;
}

export interface IPipelineDetails {
    sbuName:string;
    sbuCode:string;
    pipeline:any;
 
}
export class PipelineDetails implements IPipelineDetails {
    sbuName:string;
    sbuCode:string;
    pipeline:any;
}



