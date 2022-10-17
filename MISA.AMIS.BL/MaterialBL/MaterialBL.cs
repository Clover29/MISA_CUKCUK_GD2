using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.BL.Exceptions;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Enum;
using MISA.CUKCUK.Common.Resources;
using MISA.CUKCUK.DL.BaseDL;
using MISA.CUKCUK.DL.MaterialDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.MaterialBL
{
    public class MaterialBL : BaseBL<Material>, IMaterialBL
    {
        #region Feild
        List<string> Errors = new List<string>();
        private IMaterialDL _materialDL;

        #endregion

        #region Constructor
        public MaterialBL(IMaterialDL materialDL) : base(materialDL)
        {
            _materialDL = materialDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// kiểm tra lựa chọn lọc
        /// </summary>
        /// <param name="keyword"> loại lọc muốn lựa chọn</param>
        /// <returns>câu query lọc tương ứng</returns>
        /// Created by: VTHYEN (29/09/2022)
        public string GetFillterOption(string keyword)
        {
            if (keyword != null)
            {
                string value = keyword.Substring(1);
                switch (keyword[0])
                {
                    case '*':
                        return $"LIKE '%{value}%'";
                    case '=':
                        return $"='{value}'";
                    case '+':
                        return $"LIKE '{value}%'";
                    case '-':
                        return $"LIKE '%{value}'";
                    case '!':
                        return $"NOT LIKE '%{value}%'";
                    default:
                        return $"LIKE '%{value}%'";
                }
            }
            return null;
        }

        /// <summary>
        /// gán giá trị muốn lọc
        /// </summary>
        /// <param name="fillter">các trường thông tin muốn lọc</param>
        /// <returns> class chứa các giá trị tương ứng với từng trường muốn lọc</returns>
        /// Created by: VTHYEN (29/09/2022)
        public MaterialFillter SetFillterValue(MaterialFillter fillter)
        {
            if (fillter != null)
            {
                fillter.MaterialCode = GetFillterOption(fillter.MaterialCode);
                fillter.MaterialName = GetFillterOption(fillter.MaterialName);
                fillter.Feature = GetFillterOption(fillter.Feature);
                fillter.UnitName = GetFillterOption(fillter.UnitName);
                fillter.CategoryName = GetFillterOption(fillter.CategoryName);
                fillter.Description = GetFillterOption(fillter.Description);
            }
            return fillter;
        }

        /// <summary>
        /// API Lấy danh sách nguyên vật liệu cho phép lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param> 
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nguyên vật liệu thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nguyên vật liệu thỏa mãn điều kiện</returns>
        /// Created by: VTHYEN (29/09/2022)
        public PagingData<Material> FillterMaterial(MaterialFillter? keyword, int pageSize, int pageNumber)
        {
            if (keyword != null)
                keyword = SetFillterValue(keyword);
            return _materialDL.FillterMaterial(keyword, pageSize, pageNumber);
        }

        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        ///  Created by: VTHYEN (04/10/2022)
        public string GetNewCode(string MaterialName)
        {
          return _materialDL.GetNewCode(MaterialName);
        }

        /// <summary>
        /// Validate dữ liệu nguyên vật liệu
        /// </summary>
        /// <param name="material">class nguyên vật liệu chứa thông tin cần validate</param>
        /// <exception cref="ValidateException">message báo lỗi tương ứng</exception>
        protected override void Validate(Material material)
        {
            if (String.IsNullOrEmpty(material.MaterialCode))
            {
                Errors.Add(Resource.NullMaterialCode);
            }
            if (String.IsNullOrEmpty(material.UnitName) || material.UnitID == Guid.Empty)
            {
                Errors.Add(Resource.NullUnit);
            }
            if (String.IsNullOrEmpty(material.MaterialName))
            {
                Errors.Add(Resource.NullMaterialName);
            }
           
            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }

        }

        #endregion
    }
}
