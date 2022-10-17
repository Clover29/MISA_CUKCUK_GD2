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
        /// <summary>
        /// ID đơn vị chuyển đổi
        /// </summary>
        public Guid ConversionUnitID { get; set; }

        /// <summary>
        /// Tên đơn vị chuyển đổi
        /// </summary>
        public string? ConversionUnitName { get; set; }

        /// <summary>
        /// ID đơn vị chuyển đổi cũ
        /// </summary>
        public Guid? OldUnitID { get; set; }

        /// <summary>
        /// phép tính
        /// </summary>
        public Calculation? Calculation { get; set; }

        /// <summary>
        /// tỷ lệ chuyển đổi
        /// </summary>
        public double? ConversionRate { get; set; }

        /// <summary>
        /// Hành động khi thao tác với conversion unit
        /// </summary>
        public ConversionUnitAction Action { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày Tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
