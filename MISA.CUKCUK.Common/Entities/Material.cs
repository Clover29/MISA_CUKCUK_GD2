using MISA.CUKCUK.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.Entities
{
    /// <summary>
    /// Nguyên vật liệu
    /// </summary>
    [Table("material")]
    public class Material
    {
        #region Property

        /// <summary>
        /// ID nguyên vật liệu
        /// </summary>
        [Key]
        public Guid MaterialID { get; set; }
 
        /// <summary>
        /// Mã nguyên vật liệu
        /// </summary>
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        public string MaterialName{ get; set; }

        /// <summary>
        /// Hạn sử dụng
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Id đơn vị
        /// </summary>
        public Guid UnitID { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Id nhóm nguyên vật liệu
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// Tên nhóm nguyên vật liệu
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Id kho ngầm định
        /// </summary>
        public Guid? StockID { get; set; }

        /// <summary>
        /// Tên kho ngầm định
        /// </summary>
        public string? StockName { get; set; }

        /// <summary>
        /// Tính chất
        /// </summary>
        public string? Feature { get; set; }

        /// <summary>
        /// Số lượng tồn kho tối thiểu
        /// </summary>
        public double? InventoryNumber { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Ngưng sử dụng
        /// </summary>
        public Status? Status { get; set; }

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

        /// <summary>
        /// Danh sách đơn vị chuyển đổi
        /// </summary>
        public List<ConversionUnit>? ConversionUnits { get; set; }

        #endregion
    }
}
