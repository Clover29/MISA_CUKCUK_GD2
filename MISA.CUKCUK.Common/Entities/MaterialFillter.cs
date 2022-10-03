using MISA.CUKCUK.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.Entities
{
    public class MaterialFillter
    {
        public MaterialFillter()
        {
        }

        public MaterialFillter(string? materialCode, string? materialName, string? feature, string? unitName, string? categoryName, string? description, Status? status)
        {
            MaterialCode = materialCode;
            MaterialName = materialName;
            Feature = feature;
            UnitName = unitName;
            CategoryName = categoryName;
            Description = description;
            Status = status;
        }

        public string? MaterialCode { get; set; }
        public string? MaterialName{ get; set; }
        public string? Feature { get; set; }
        public string? UnitName { get; set; }

        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public Status? Status { get; set; }

        public override string ToString()
        {
            return MaterialCode + MaterialName + Feature + UnitName + CategoryName + Description + Status;
        }
    }
}
