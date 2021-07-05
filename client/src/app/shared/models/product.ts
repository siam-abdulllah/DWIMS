
export interface IProduct {
    id: number;
    ProductName: string;
    ProductCode: string;
    status: string; 
    setOn: Date;
}
 
export class Product implements IProduct {
    id: number=0;
    ProductName: string;
    ProductCode: string;
    status: string="Active"; 
    setOn: Date;
}