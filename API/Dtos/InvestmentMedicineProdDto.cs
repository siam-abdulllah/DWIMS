using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvestmentMedicineProdDto
    {
        public int id { get; set; }
        public int investmentInitId { get; set; }
        public int productId { get; set; }
        public double tpVat { get; set; }
        public int employeeId { get; set; }
        public int boxQuantity { get; set; }
        public MedicineProduct medicineProduct { get; set; }
  
    }
}
