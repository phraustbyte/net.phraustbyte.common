using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Helper class for DBISam
    /// </summary>
    public static class  DBISamHelper
    {
        /// <summary>
        /// Generates an insert statement using reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GenerateInsertStatment<T> (T obj)
        {

            //TableAttribute attrib = obj.GetType().GetCustomAttributes(typeof(TableAttribute),false).First(x=>x.)
            object[] attribs = obj.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            var attr = attribs.First() as TableAttribute;
            string Table = attr.TableName;
            bool InsertViaSelect = obj.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(InsertViaSelectAttribute));
            List<string> PropertyList = new List<string>();
            List<string> ValueList = new List<string>();
            var propertyList = obj.GetType().GetProperties();
            foreach (var prop in propertyList)
            {
                if (!(prop.CustomAttributes.Any(x=>x.AttributeType == typeof(IgnoreAttribute))))
                {
                    PropertyList.Add(prop.Name);
                    /*if (prop.CustomAttributes.Any(x=>x.AttributeType == typeof(AutoIncAttribute)))
                    {
                        ValueList.Add($"LastAutoInc('{Table}') + 1");
                    }
                    else*/ if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DateAttribute)))
                        ValueList.Add($"CAST('{((DateTime)prop.GetValue(obj, null)).ToString("yyyy-MM-dd")}' AS DATE)");
                    else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(TimeAttribute)))
                        ValueList.Add($"CAST('{((TimeSpan)prop.GetValue(obj, null)).ToString("c")}' AS TIME)");
                    else if (prop.PropertyType == typeof(string))
                    {
                        var str = ((string)prop.GetValue(obj, null)) ?? "";
                        // str = str.Replace("\"", "\\\"");
                        str = str.Replace("'", "'+#39+'");
                        ValueList.Add($"'{str}'");
                    }
                    else if (prop.PropertyType == typeof(int))
                        ValueList.Add(prop.GetValue(obj, null).ToString());
                    else if (prop.PropertyType == typeof(float))
                        ValueList.Add(((float)prop.GetValue(obj, null)).ToString());
                    else if (prop.PropertyType == typeof(bool))
                        ValueList.Add(((bool)prop.GetValue(obj, null))?"True":"False");
                    else if (prop.PropertyType == typeof(DateTime))
                        ValueList.Add($"CAST('{((DateTime)prop.GetValue(obj, null)).ToString("yyyy-MM-dd hh:mm:ss")}' AS TIMESTAMP)");
                }
            }
            string Query = "";
            if (InsertViaSelect)
                Query =  $"INSERT INTO {Table} ({String.Join(",",PropertyList)}) SELECT {string.Join(",",ValueList)} FROM {Table};";
            else
                Query = $"INSERT INTO {Table} ({String.Join(",", PropertyList)}) VALUES ({string.Join(",", ValueList)});";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            Query += $" SELECT LASTAUTOINC('{Table}') FROM {Table} TOP 1;";
            return Query;
        }
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long ConvertToTimestamp (this DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
    }
}
