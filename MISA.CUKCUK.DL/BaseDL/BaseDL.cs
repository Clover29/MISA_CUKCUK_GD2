using Dapper;
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
            string tableName = EntityUltilities.getTableName<T>();
            string storedProcedureName = $"Proc_{tableName}_Insert";
            var propertyID = $"v_{tableName}ID";
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            var recordID = Guid.NewGuid();
            foreach (var property in properties)
            {

                string propertyName = $"v_{property.Name}";
                if (property.Name != "ConversionUnits")
                {
                    var propertyValue = (propertyName.ToLower() == propertyID.ToLower()) ? recordID : property.GetValue(record);

                    parameters.Add(propertyName, propertyValue);
                }


            }
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                return mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
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
