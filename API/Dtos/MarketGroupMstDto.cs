using System;

namespace API.Dtos
{
    public class MarketGroupMstDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Status { get; set; }

        //public MarketGrpEmployeeDto Employee { get; set; }
        public MarketGroupDtlDto MarketGroupDtlDto { get; set; }
        public int  EmployeeId { get; set; }
       
    }
}