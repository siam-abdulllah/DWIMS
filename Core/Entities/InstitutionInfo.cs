﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class InstitutionInfo : BaseEntity
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionType { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}
