using MISA.CUKCUK.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Common.DTO
{
    public class ErrorResult
    {
        #region Property

        /// <summary>
        /// Mã lỗi được định danh
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Thông báo gửi tới người dùng
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông báo gửi về phía dev
        /// </summary>
        public object? DevMsg { get; set; }

        /// <summary>
        /// Mã để tra cứu thông tin log: ELK, file log,...
        /// </summary>
        public string? TraceId { get; set; }

        #endregion

        #region Constructor
        public ErrorResult()
        {
        }

        public ErrorResult(ErrorCode errorCode, string? userMsg, object? devMsg, string? traceId)
        {
            ErrorCode = errorCode;
            UserMsg = userMsg;
            DevMsg = devMsg;
            TraceId = traceId;
        }

        #endregion
    }
}
