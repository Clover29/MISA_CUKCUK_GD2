using Dapper;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.Common.Ultilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method
        public int DeleteOneRecord(Guid recordID)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                string className = typeof(T).Name;
                string storedProcedureName = $"Proc_{className}_Delete"; ;

                var parameters = new DynamicParameters();
                parameters.Add($"@v_{className}ID", recordID);

                return mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> getAllRecords()
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                string className = typeof(T).Name;
                string storedProcedureName = $"Proc_{className}_GetAll";

                var records = mySqlConnection.Query<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                return records;
            }
        }

        public virtual T GetRecordByID(Guid recordID)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                string className = typeof(T).Name;

                string storedProcedureName = $"Proc_{className}_GetById";
                var parameters = new DynamicParameters();
                parameters.Add($"@v_{className}ID", recordID);
                return mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public int InsertOneRecord(T record)
        {
            int numberOfAffectedRows = 0;
            string tableName = EntityUltilities.getTableName<T>();
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
            return numberOfAffectedRows;
        }

        public int UpdateOneRecord(T record, Guid recordID)
        {
            string tableName = EntityUltilities.getTableName<T>();
            string storedProcedureName = $"Proc_{tableName}_Update";
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
        #endregion

    }
}
