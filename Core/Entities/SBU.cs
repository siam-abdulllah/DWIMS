﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class SBU : BaseEntity
    {
        public string SBUName { get; set; }
        public string SBUCode { get; set; }
        public string Remarks { get; set; }
    }
    public class SBUVM : SBU
    {
        public Int64 SBUAmount { get; set; }
        public int BgtSbuId { get; set; }
        public double Expense { get; set; }
        public double TotalAllowcated { get; set; }
    }
}
