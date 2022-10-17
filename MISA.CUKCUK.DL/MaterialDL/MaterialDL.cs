using Dapper;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Enum;
using MISA.CUKCUK.Common.Ultilities;
using MISA.CUKCUK.DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.CUKCUK.DL.MaterialDL
{
    public class MaterialDL : BaseDL<Material>, IMaterialDL
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
        public PagingData<Material> FillterMaterial(MaterialFillter? keyword, int pageSize, int pageNumber)
        {
            string storedprocedurename = "Proc_material_GetPaging";

            var parameters = new DynamicParameters();
            parameters.Add("@v_offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_limit", pageSize);
            parameters.Add("@v_sort", "modifieddate desc");
            var andConditions = new List<string>();
            string whereclause = "";
            var pagingData = new PagingData<Material>();
            if (keyword != null)
            {
                if (!string.IsNullOrEmpty(keyword.MaterialCode)) andConditions.Add($"MaterialCode {keyword.MaterialCode}");
                if (!string.IsNullOrEmpty(keyword.MaterialName)) andConditions.Add($"MaterialName {keyword.MaterialName}");
                if (!string.IsNullOrEmpty(keyword.Feature)) andConditions.Add($"Feature {keyword.Feature}");
                if (!string.IsNullOrEmpty(keyword.UnitName)) andConditions.Add($"UnitName {keyword.UnitName}");
                if (!string.IsNullOrEmpty(keyword.CategoryName)) andConditions.Add($"CategoryName {keyword.CategoryName}");
                if (!string.IsNullOrEmpty(keyword.Description)) andConditions.Add($"Description {keyword.Description}");
                if (keyword.Status != null)
                {
                    var status = keyword.Status.Value.Equals(MISA.CUKCUK.Common.Enum.Status.Using) ? 1 : 2;
                    andConditions.Add($"Status = '{status}'");
                }
            }
            if (andConditions.Count > 0)
            {
                whereclause = $"({string.Join(" and ", andConditions)})";

            }
            parameters.Add("@v_where", whereclause);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var multipleResults = mySqlConnection.QueryMultiple(storedprocedurename, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (multipleResults != null)
                {
                    pagingData.Data = multipleResults.Read<Material>().ToList();
                    pagingData.TotalRecord = multipleResults.Read<long>().Single();
                    if (pagingData.TotalRecord % pageSize != 0)
                    {
                        pagingData.TotalPage = (int)((pagingData.TotalRecord / pageSize) + 1);
                    }
                    else
                    {
                        pagingData.TotalPage = (int)pagingData.TotalRecord / pageSize;
                    }
                    pagingData.CurrentPage = pageNumber;
                    pagingData.CurrentPageRecords = pageSize;

                }
                return pagingData;
            }
        }


        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        ///  <param name="materialName">Tên nguyên vật liệu</param> 
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        ///  Created by: VTHYEN (04/10/2022)
        public string GetNewCode(string materialName)
        {
            string newCode = "";
            string[] materialNameSubStrings = materialName.Split(" ");
            if (materialNameSubStrings.Length > 1)
            {
                for (int i = 0; i < materialNameSubStrings.Length; i++)
                {
                    newCode += materialNameSubStrings[i][0];
                }
            }
            else
            {
                if (materialNameSubStrings[0].Length > 1)
                {
                    newCode = materialNameSubStrings[0].Substring(0, 2);
                }

            }
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                string storedProcedureName = "Proc_material_GetNewCode";
                string maxEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                return newCode.ToUpper() + "-" + (maxEmployeeCode.ToString());
            }
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        ///  <param name="recordID">id bản ghi cần lấy</param> 
        /// <returns>
        /// bản ghi chứa thông tin nguyên vật liệu
        /// </returns>
        ///  Created by: VTHYEN (04/10/2022)
        public override Material GetRecordByID(Guid recordID)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                string materialProcedure = $"Proc_material_GetById";
                string conversionUnitProcedure = $"Proc_conversionunit_GetAll";
                var parameters = new DynamicParameters();
                parameters.Add("v_MaterialID", recordID);
                var material = mySqlConnection.QueryFirstOrDefault<Material>(materialProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var conversionUnit = mySqlConnection.Query<ConversionUnit>(conversionUnitProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (conversionUnit.Count() > 0)
                {
                    foreach (var item in conversionUnit)
                    {
                        item.OldUnitID = item.ConversionUnitID;
                    }
                    material.ConversionUnits = (List<ConversionUnit>?)conversionUnit;
                }

                return material;
            }
        }
        /// <summary>
        /// API Sửa 1 bản ghi
        /// </summary>
        /// <param name="materialId">ID của bản ghi muốn sửa</param>
        /// <param name="material">bản ghi muốn sửa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public override int UpdateOneRecord(Material material, Guid materialId)
        {
            int numberOfAffectedRows = 0;
            string tableName = EntityUltilities.GetTableName<Material>();
            string storedProcedureName = $"Proc_material_Update";
            var propertyID = $"v_{tableName}ID";
            var properties = typeof(Material).GetProperties();
            var parameters = new DynamicParameters();
            List<ConversionUnit> unitValue = new List<ConversionUnit>();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";

                if (property.Name == "ConversionUnits")
                {
                    unitValue = (List<ConversionUnit>)property.GetValue(material);
                    continue;
                }
                var propertyValue = ((propertyName.ToLower() == propertyID.ToLower()) ? materialId : property.GetValue(material));

                parameters.Add(propertyName, propertyValue);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                if (unitValue != null)
                {
                    foreach (ConversionUnit unit in unitValue)
                    {
                        var conversionUnitParameters = new DynamicParameters();
                        string procedureConversionUnit = "";
                        switch (unit.Action)
                        {
                            case ConversionUnitAction.Delete:
                                procedureConversionUnit = "Proc_conversionunit_Delete";
                                break;
                            case ConversionUnitAction.Insert:
                                procedureConversionUnit = "Proc_conversionunit_Insert";
                                break;
                            case ConversionUnitAction.Update:
                                procedureConversionUnit = "Proc_conversionunit_Update";
                                break;
                            default:
                                procedureConversionUnit = "Proc_conversionunit_Update";
                                break;
                        }
                        var ConversionUnitProperties = typeof(ConversionUnit).GetProperties();
                        string variableName = "v_MaterialID";
                        var variableValue = materialId;
                        conversionUnitParameters.Add(variableName, variableValue);
                        foreach (var item in ConversionUnitProperties)
                        {
                            string itemName = $"v_{item.Name}";
                            var itemValue = item.GetValue(unit);
                            conversionUnitParameters.Add(itemName, itemValue);
                        }
                        using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                        {
                            numberOfAffectedRows += mySqlConnection.Execute(procedureConversionUnit, conversionUnitParameters, commandType: System.Data.CommandType.StoredProcedure);
                        }
                    }
                }
                scope.Complete();
            }
            return numberOfAffectedRows;
        }
    }
}
