export interface IRole {
    id: string;
    roleName: string;
}

export interface IRoleResponse {
  pageIndex: number;
  pageSize: number;
  count: number;
  data:IRole[];
}
