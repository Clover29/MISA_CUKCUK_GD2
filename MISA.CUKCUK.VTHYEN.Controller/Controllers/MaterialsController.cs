using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.BL.Exceptions;
using MISA.AMIS.BL.MaterialBL;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Enum;
using MISA.CUKCUK.Common.Resources;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace MISA.CUKCUK.VTHYEN.Controller.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MaterialsController : BasesController<Material>
    {
        #region Field

        private IMaterialBL _materialBL;

        #endregion

        #region Constructor
        public MaterialsController(IMaterialBL materialBL) : base(materialBL)
        {
            _materialBL = materialBL;
        }
        #endregion
        #region Method
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
        [HttpGet("Fillter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Material>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FillterMaterials([FromQuery] string? MaterialCode,
            [FromQuery] string? MaterialName,
            [FromQuery] string? Feature,
            [FromQuery] string? UnitName,
            [FromQuery] string? CategoryName,
            [FromQuery] string? Description,
            [FromQuery] Status? Status,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                MaterialFillter fillter = new MaterialFillter(MaterialCode, MaterialName, Feature, UnitName, CategoryName, Description, Status);

                var pagingData = _materialBL.FillterMaterial(fillter, pageSize, pageNumber);

                if (pagingData != null)
                {

                    return StatusCode(StatusCodes.Status200OK, pagingData);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                var errorResult = new ErrorResult(
                 ErrorCode.Exception,
                Resource.OtherException,
                 exception.Message,
                 Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }

        }

        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        ///  Created by: VTHYEN (04/10/2022)
        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewEmployeeCode(string MatterialName)
        {
            try
            {
                string newCode = _materialBL.GetNewCode(MatterialName);
                return StatusCode(StatusCodes.Status200OK, newCode);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                var errorResult = new ErrorResult(
                  ErrorCode.Exception,
                 Resource.OtherException,
                  exception.Message,
                  Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }

        }
        #endregion
    }
}
