export interface IInvestmentRecDepot {
    id: number;
    investmentInitId: number;
    depotCode: string;
    depotName: string;
    employeeId: number;
}
 
export class InvestmentRecDepot implements IInvestmentRecDepot {
    id: number=0;
    investmentInitId: number;
    depotCode: string;
    depotName: string;
    employeeId: number;
}


