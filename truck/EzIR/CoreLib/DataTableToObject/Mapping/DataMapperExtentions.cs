using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CoreLib.DataTableToObject.Mapping
{
    public static class DataMapperExtentions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToListItem<T>(this DataSet dataSet) where T : class, new()
        {
            if (dataSet.Tables.Count > 1)
            {
                throw new Exception("Dataset must contain only 1 table to convert to collection");
            }

            if (dataSet.Tables.Count == 0)
            {
                return new List<T>();
            }

            var mapper = new DataNamesMapper<T>();
            return mapper.Map(dataSet.Tables[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToListItem<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null)
            {
                throw new NullReferenceException("Datatable cannot be null when convert to list item");
            }

            var mapper = new DataNamesMapper<T>();
            return mapper.Map(dataTable);
        }

        public static IEnumerable<T> ToGenericList<T>(this DataTable dataTable) where T : class, new()
        {
            var properties = typeof(T).GetProperties().Where(x => x.CanWrite).ToList();

            var result = new List<T>();

            // loop on rows
            foreach (DataRow row in dataTable.Rows)
            {
                // create an instance of T generic type.
                var item = Activator.CreateInstance<T>();

                // loop on properties and columns that matches properties
                foreach (var prop in properties)
                    foreach (DataColumn column in dataTable.Columns)
                        if (prop.Name.ToLower() == column.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = row[column.ColumnName];
                            // Set the value into the object
                            prop.SetValue(item, value);
                            break;
                        }
                result.Add(item);
            }

            return result;
        }
    }
}
