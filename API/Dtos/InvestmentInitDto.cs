using Core.Entities;
using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class InvestmentInitDto
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
        public string DonationTo { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeDto EmployeeDto { get; set; }
        public string PostingType { get; set; }
        public string MarketGroupCode { get; set; }
        public string MarketGroupName { get; set; } 
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
        public string SBUName { get; set; }
        public bool Confirmation { get; set; }
        public DateTimeOffset? SubmissionDate { get; set; }
    }
}