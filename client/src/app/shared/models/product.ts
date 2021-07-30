
export interface IProduct {
    id: number;
    productName: string;
    productCode: string;
    status: string; 
    setOn: Date;
}
 
export class Product implements IProduct {
    id: number=0;
    productName: string;
    productCode: string;
    status: string="Active"; 
    setOn: Date;
}