
export interface IInvestmentInit {
    id: number;
    referenceNo: string;
    proposeFor: string;
    donationType: string;
    donationTo: string;
    EmployeeId: number;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;
    referenceNo: string;
    proposeFor: string;
    donationType: string;
    donationTo: string;
    EmployeeId: number;
}
export interface IInvestmentDetail {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    totalMonth: string;
    proposedAmount: string;
    purpose: string;
    FromDate: Date;
    ToDate: Date;
    subCampEndDate: Date;
}
 
export class InvestmentDetail implements IInvestmentDetail {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    totalMonth: string;
    proposedAmount: string;
    purpose: string;
    FromDate: Date;
    ToDate: Date;
    subCampEndDate: Date;
}
export interface IInvestmentTargetedProd {
    id: number;
    investmentInitId: number;
    marketGroupId: number;
    productName: string;
    
}
 
export class InvestmentTargetedProd implements IInvestmentTargetedProd {
    id: number=0;
    investmentInitId: number;
    marketGroupId: number;
    productName: string;
}