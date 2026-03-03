using System.Data;
using System.Reflection;

namespace TaskManagement.Services.Common
{
    public static class BLGlobalClass
    {
        public static DataTable ToDataTable<T>(this List<T> lstData)
        {
            DataTable dt = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dt.Columns.Add(prop.Name, propType);
            }

            foreach (T item in lstData)
            {
                object[] values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}
