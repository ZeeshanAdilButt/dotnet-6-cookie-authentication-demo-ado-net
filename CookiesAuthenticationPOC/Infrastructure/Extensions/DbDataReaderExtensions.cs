using CookiesAuthenticationPOC.Infrastructure.Extensions;
using System.Data.Common;

namespace CookiesAuthenticationPOC.Infrastructure.Extensions
{
    /// <summary>
    /// DbDataReaderExtensions contains extensions for data convertion for DbDataReader.
    /// </summary>
    public static class DbDataReaderExtensions
    {
       

        /// <summary>
        /// GetStringValue get value from DataReader for provided ColumnName and converts it to string.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetStringValue(this DbDataReader dataReader, string columnName)
        {
            var objVal = dataReader.GetValue(columnName);

            return objVal.GetStringValue();
        }

        /// <summary>
        /// GetValue get value from DataReader for provided ColumnName and returns as object.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static object GetValue(this DbDataReader dataReader, string columnName)
        {
            object val = null;

            if (dataReader[columnName] != DBNull.Value)
            {
                val = dataReader[columnName];
            }

            return val;
        }
    }
}