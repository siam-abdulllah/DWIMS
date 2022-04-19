export interface ISBUWiseExpSummaryReport {
    sbu: string;
    sbuName: string;
    donationTypeName: string;
    donationId: string;
    expense: number;
    budget: number;
}

export interface IEmpWiseExpSummaryReport {
    employeeId: string;
    employeeName: string;
    duration: string;
    donationTypeName: string;
    donationId: string;
    expense: number;
    budget: number;
}
