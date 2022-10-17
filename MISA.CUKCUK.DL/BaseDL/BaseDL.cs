using Dapper;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Ultilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.CUKCUK.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns> Tất cả bản ghi của một bảng</returns>
        /// Created by: YENVTH (29/09/2022)
        public IEnumerable<T> GetAllRecords()
        {
            string className = typeof(T).Name;
            string storedProcedureName = $"Proc_{className}_GetAll";

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var records = mySqlConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                return records;
            }
        }

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 bản ghi
        /// </summary>
        /// <param name="ID">ID của bản ghi muốn lấy thông tin chi tiết</param>
        /// <returns>bản ghi muốn lấy thông tin chi tiết</returns>
        /// Created by: VTHYEN (30/09/2022)
        public virtual T GetRecordByID(Guid recordID)
        {
            string className = typeof(T).Name;

            string storedProcedureName = $"Proc_{className}_GetById";
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{className}ID", recordID);
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                return mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// API Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">bản ghi muốn thêm mới</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public int InsertOneRecord(T record)
        {

            int numberOfAffectedRows = 0;
            string tableName = EntityUltilities.GetTableName<T>();
            string storedProcedureName = $"Proc_{tableName}_Insert";
            var propertyID = $"v_{tableName}ID";
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            var recordID = Guid.NewGuid();
            List<ConversionUnit> unitValue = new List<ConversionUnit>();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                if (property.Name == "ConversionUnits")
                {
                    unitValue = (List<ConversionUnit>)property.GetValue(record);
                    continue;
                }
                var propertyValue = (propertyName.ToLower() == propertyID.ToLower()) ? recordID : property.GetValue(record);
                parameters.Add(propertyName, propertyValue);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    numberOfAffectedRows += mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                if (unitValue != null)
                {
                    var conversionUnitParameters = new DynamicParameters();
                    foreach (ConversionUnit unit in unitValue)
                    {
                        string procedureConversionUnit = "Proc_conversionunit_Insert";
                        var ConversionUnitProperties = typeof(ConversionUnit).GetProperties();
                        string variableName = "v_MaterialID";
                        var variableValue = recordID;
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

        /// <summary>
        /// API Sửa 1 bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn sửa</param>
        /// <param name="record">bản ghi muốn sửa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public virtual int UpdateOneRecord(T record, Guid recordID)
        {
            string tableName = EntityUltilities.GetTableName<T>();
            string storedProcedureName = $"Proc_{tableName}_Update{tableName}";
            var propertyID = $"v_{tableName}ID";
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";

                var propertyValue = ((propertyName.ToLower() == propertyID.ToLower()) ? recordID : property.GetValue(record));

                parameters.Add(propertyName, propertyValue);
            }
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                return mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// API Xóa 1 bản ghi
        /// </summary>
        /// <param name="recordID">ID của bản ghi cần xóa</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created by: VTHYEN (30/09/2022)
        public int DeleteOneRecord(Guid recordID)
        {
            string className = typeof(T).Name;
            string storedProcedureName = $"Proc_{className}_Delete"; ;

            var parameters = new DynamicParameters();
            parameters.Add($"@v_{className}ID", recordID);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                return mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        #endregion

    }
}
