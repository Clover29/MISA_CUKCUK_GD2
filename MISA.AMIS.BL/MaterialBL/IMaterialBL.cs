using MISA.AMIS.BL.BaseBL;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.MaterialBL
{
    public interface IMaterialBL:IBaseBL<Material>
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
        /// Created by: VTHYEN (29/09/2022)
        public PagingData<Material> FillterMaterial(MaterialFillter? keyword, int pageSize, int pageNumber);
    }
}
