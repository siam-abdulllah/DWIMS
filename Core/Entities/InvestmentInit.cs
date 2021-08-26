using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentInit : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }

        public string DonationType { get; set; }
        public string DonationTo { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string PostingType { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SBU { get; set; }

    }
}
