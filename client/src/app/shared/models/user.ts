export interface IUser {
    id:string;
    employeeId: number;
    email: string;
    displayName: string;
    token: string;
}

export interface IUserResponse {
    id: string;
    email: string;
    displayName: string;
    phoneNumber: string;
    emailConfirmed: string;
    token: string;
    roles:[];
}