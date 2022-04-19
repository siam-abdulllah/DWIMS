
export interface IProduct {
    id: number;
    productName: string;
    productCode: string;
    status: string; 
    sbu: string; 
    sbuName: string; 
    setOn: Date;
}
 
export class Product implements IProduct {
    id: number=0;
    productName: string;
    productCode: string;
    status: string="Active"; 
    sbu: string; 
    sbuName: string; 
    setOn: Date;
}