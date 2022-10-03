using Dapper;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.DL.MaterialDL
{
    public class MaterialDL : BaseDL<Material>, IMaterialDL
    {
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

        public override Material GetRecordByID(Guid recordID)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                string storedProcedureName = $"Proc_material_GetById";
                var parameters = new DynamicParameters();
                parameters.Add("v_MaterialID", recordID);
                var records = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var material = records.Read<Material>().Single();
                var convesionunit = records.Read<ConversionUnit>().ToList();
                material.ConversionUnits = convesionunit;
                return material;
            }
        }

    }
}
