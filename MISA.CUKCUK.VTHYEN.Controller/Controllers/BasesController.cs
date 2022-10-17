﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.BL.Exceptions;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Enum;
using MISA.CUKCUK.Common.Resources;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Diagnostics;

namespace MISA.CUKCUK.VTHYEN.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất Cả bản ghi</returns>
        /// Created By: YenVTH (23/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult getAllRecords()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetAllRecords());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var errorResult = new ErrorResult(
                  ErrorCode.Exception,
                 Resource.OtherException,
                  ex.Message,
                  Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }
        }

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 bản ghi
        /// </summary>
        /// <param name="ID">ID của bản ghi muốn lấy thông tin chi tiết</param>
        /// <returns>bản ghi muốn lấy thông tin chi tiết</returns>
        /// Created by: VTHYEN (30/09/2022)
        [HttpGet("{ID}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetRecordByID([FromRoute] Guid ID)
        {
            try
            {
                var records = _baseBL.GetRecordByID(ID);
                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
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
        /// API Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi muốn thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: VTHYEN (30/09/2022)
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertOneRecord([FromBody] T record)
        {
            try
            {
                int numberOfAffectedRows = _baseBL.InsertOneRecord(record);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, 1);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch (ValidateException ex)
            {
                var errorResult = new ErrorResult(
                    ErrorCode.Validate,
                    ex.Message,
                    ex.Data,
                    Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status400BadRequest, errorResult);
            }
            catch (MySqlException mySqlException)
            {
                var errorResult = new ErrorResult(
                   ErrorCode.DuplicateCode,
                  Resource.DuplicateMaterialCode,
                    mySqlException.Message,
                   Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResult);
                }
                return StatusCode(StatusCodes.Status400BadRequest, errorResult);
            }
            catch (Exception exception)
            {
                var errorResult = new ErrorResult(
                  ErrorCode.Exception,
                 Resource.OtherException,
                  exception.Message,
                  Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }
        }

        /// <summary>
        /// API Sửa 1 bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn sửa</param>
        /// <param name="record">bản ghi muốn sửa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        [HttpPut("{recordID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOneRecord([FromBody] T record, [FromRoute] Guid recordID)
        {
            try
            {
                int numberOfAffectedRows = _baseBL.UpdateOneRecord(record, recordID);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, 1);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (ValidateException ex)
            {
                var errorResult = new ErrorResult(
                    ErrorCode.Validate,
                    ex.Message,
                    ex.Data,
                    Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status400BadRequest, errorResult);
            }
            catch (MySqlException mySqlException)
            {
                var errorResult = new ErrorResult(
                   ErrorCode.DuplicateCode,
                  Resource.DuplicateMaterialCode,
                    mySqlException.Message,
                   Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResult);
                }
                return StatusCode(StatusCodes.Status400BadRequest, errorResult);
            }
            catch (Exception exception)
            {
                var errorResult = new ErrorResult(
                  ErrorCode.Exception,
                 Resource.OtherException,
                  exception.Message,
                  Activity.Current?.Id ?? HttpContext?.TraceIdentifier);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }
        }
        /// <summary>
        /// API Xóa 1 nhân viên
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: VTHYEN (30/09/2022)
        [HttpDelete("{recordID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteOneRecord([FromRoute] Guid recordID)
        {
            try
            {
                int numberOfAffectedRows = _baseBL.DeleteOneRecord(recordID);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
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
                return StatusCode(StatusCodes.Status400BadRequest, errorResult);
            }

        }
        #endregion
    }
}
