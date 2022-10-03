using MISA.CUKCUK.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.Entities
{
    public class ConversionUnit
    {
        public Guid ConversionUnitID { get; set; }
        public string ConversionUnitName { get; set; }
        public Calculation Calculation { get; set; }
        public double ConversionRate { get; set; }
    }
}
