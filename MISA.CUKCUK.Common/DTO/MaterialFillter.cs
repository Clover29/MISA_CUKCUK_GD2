using MISA.CUKCUK.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.DTO
{
    public class MaterialFillter
    {
        #region Properties

        /// <summary>
        /// Mã Nguyên vật liệu
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// Tính chất
        /// </summary>
        public string? Feature { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string? UnitName { get; set; }

        /// <summary>
        /// Tên nhóm nguyên vật liệu
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Tình trạng
        /// </summary>
        public Status? Status { get; set; }

        #endregion

        #region Constructor
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


        #endregion

    }
}
