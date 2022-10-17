using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.Entities
{
    /// <summary>
    /// Kho ngầm định
    /// </summary>
    [Table("stock")]
    public class Stock
    {
        /// <summary>
        /// ID Kho
        /// </summary>
        public Guid StockID { get; set; }

        /// <summary>
        /// Mã Kho
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// Tên Kho
        /// </summary>
        public string StockName { get; set; }

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
