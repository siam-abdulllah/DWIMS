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
    newAmount:any;
    addAmount:any;
    totalExpense:any;
    totalPipeline:any;
    totalRemaining:any;
    totalAllocated:any;
   
}
 
export class BudgetYearly implements IBudgetYearly {
    
    id: number=0;
    compId:number=1000;
    deptId:number;
    year:number =2022;
    totalAmount:any;
    remarks:string;
    enteredBy:number;
    newAmount:any;
    addAmount:any;
    totalExpense:any;
    totalPipeline:any;
    totalRemaining:any;
    totalAllocated:any;

}

export interface IBudgetSbuYearly {
    id: number;
    bgtSbuId:number;
    compId:number;
    deptId:number;
    year:number;
    sbuCode:string;
    sbuName:string;
    sbuAmount:any;
    remarks:string;
    enteredBy:number;
    totalBudget:any;
    remainingBudget:any;
    expense:any;
    totalAllowcated:any;
    totalExpense:any;
    sbuDetailsList:ISbuDetails[];
   
}
 
export class BudgetSbuYearly implements IBudgetSbuYearly {
    id: number=0;
    bgtSbuId:number;
    compId:number=1000;
    deptId:number=0;
    year:number;
    sbuCode:string=null;
    sbuName:string;
    sbuAmount:number;
    remarks:string;
    enteredBy:number;
    totalBudget:any;
    remainingBudget:any;
    expense:any;
    totalAllowcated:any;
    totalExpense:any;
    sbuDetailsList:ISbuDetails[];

}

export interface ISbuDetails {
    bgtSbuId:number;
    sbuName:string;
    sbuCode:string;
    sbuAmount:any;
    newAmount:any;
    expense:any;
    pipeLine:any;
    totalAlowcated:any;
    remaining:any;
}
export class SbuDetails implements ISbuDetails {
    bgtSbuId:number;
    sbuName:string;
    sbuCode:string;
    sbuAmount:any;
    newAmount:any;
    expense:number;
    totalAlowcated:any;
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

export interface IAuthExpense {
    remarks:string;
    expense:any;

 
}
export class AuthExpense implements IAuthExpense {
    remarks:string;
    expense:any;
}

export interface ICampaignBgtDetails {
    id:number;
    noOfCamp:number;
    allocatedBgt:any;
    expense:any;
    amount:any;
    newAmount:any;
    deptId:number;
    sbu:string;
    sbuCode:string;
    year:number;
    compId:number;
    enteredBy : string;
}
export class CampaignBgtDetails implements ICampaignBgtDetails{
    id:number;
    noOfCamp:number;
    allocatedBgt:any;
    expense:any;
    amount:any;
    newAmount:any;
    deptId:number;
    sbu:string;
    sbuCode:string;
    year:number;
    compId:number;
    enteredBy : string;
}
export interface IApprovalAuthDetails {
    id:number;
    bgtEmpId:number;
    authority:string;
    authId:number;
    noOfEmployee:number;
    noOfLoc:number;
    priority:number;
    expense:any;
    amount:any;
    newAmount:any;
    deptId:number;
    sbu:string;
    sbuCode:string;
    year:number;
    compId:number;
    permView:boolean;
    permEdit:boolean;
    enteredBy : string;
}
export class ApprovalAuthDetails implements IApprovalAuthDetails {
    id:number;
    bgtEmpId:number;
    authority:string;
    authId:number;
    noOfEmployee:number;
    noOfLoc:number;
    priority:number;
    expense:any;
    amount:any;
    newAmount:any;
    deptId:number;
    sbu:string;
    sbuCode:string;
    year:number;
    compId:number;
    permView:boolean;
    permEdit:boolean;
    enteredBy : string;
}

export interface IApprovalAuthDetailsModel {
    deptId:number;
    sbu:string;
    year:number;
    compId:number;
    bgtEmpList : ApprovalAuthDetails[];
}
export class ApprovalAuthDetailsModel implements IApprovalAuthDetailsModel {
 
    deptId:number;
    sbu:string;
    sbuCode:string;
    year:number;
    compId:number;
    bgtEmpList : ApprovalAuthDetails[];
}





