using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.Enum
{
    /// <summary>
    /// Mã lỗi
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Lỗi chưa xác định được
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi validate dữ liệu k đúng format
        /// </summary>
        Validate = 2,

        /// <summary>
        /// Lỗi trùng mã
        /// </summary>
        DuplicateCode = 3,
    }
}
