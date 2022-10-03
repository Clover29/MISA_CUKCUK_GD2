﻿using MISA.CUKCUK.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.DL.BaseDL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns> Tất cả bản ghi của một bảng</returns>
        /// Created by: YENVTH (29/09/2022)
        public IEnumerable<T> getAllRecords();

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 bản ghi
        /// </summary>
        /// <param name="ID">ID của bản ghi muốn lấy thông tin chi tiết</param>
        /// <returns>bản ghi muốn lấy thông tin chi tiết</returns>
        /// Created by: VTHYEN (30/09/2022)
        public T GetRecordByID(Guid ID);

        /// <summary>
        /// API Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">bản ghi muốn thêm mới</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public int InsertOneRecord(T record);

        /// <summary>
        /// API Sửa 1 bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn sửa</param>
        /// <param name="record">bản ghi muốn sửa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public int UpdateOneRecord(T record, Guid recordID);

        /// <summary>
        /// API Xóa 1 bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi cần xóa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public int DeleteOneRecord(Guid recordID);
    }
}
