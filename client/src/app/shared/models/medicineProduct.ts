
export interface IMedicineProduct {
    id: number;
    productName: string;
    productCode: string;
    status: string; 
    unitTp: number; 
    unitVat: number; 
    sorgaCode: Date;
}
 
export class MedicineProduct implements IMedicineProduct {
    id: number=0;
    productName: string;
    productCode: string;
    status: string="Active"; 
    unitTp: number; 
    unitVat: number; 
    sorgaCode: Date;
}