using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Enum;
using MISA.CUKCUK.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.DL.MaterialDL
{
    public interface IMaterialDL:IBaseDL<Material>
    {

        /// <summary>
        /// API Lấy danh sách nguyên vật liệu cho phép lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param> 
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nguyên vật liệu thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nguyên vật liệu thỏa mãn điều kiện</returns>
        /// Created by:  VTHYEN (29/09/2022)
        public PagingData<Material> FillterMaterial(MaterialFillter? keyword, int pageSize, int pageNumber);

        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        ///  Created by: VTHYEN (04/10/2022)
        public string GetNewCode(string MaterialName);

       
    }
}
