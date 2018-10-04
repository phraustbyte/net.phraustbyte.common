using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.phraustbyte.dal.dbisam
{
    public static class  DBISamHelper
    {
        public static string GenerateInsertStatment<T> (T obj)
        {

            //DBISamTableAttribute attrib = obj.GetType().GetCustomAttributes(typeof(DBISamTableAttribute),false).First(x=>x.)
            object[] attribs = obj.GetType().GetCustomAttributes(typeof(DBISamTableAttribute), true);
            var attr = attribs.First() as DBISamTableAttribute;
            string Table = attr.TableName;
            bool InsertViaSelect = obj.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(DBISamInsertViaSelectAttribute));
            List<string> PropertyList = new List<string>();
            List<string> ValueList = new List<string>();
            var propertyList = obj.GetType().GetProperties();
            foreach (var prop in propertyList)
            {
                if (!(prop.CustomAttributes.Any(x=>x.AttributeType == typeof(DBISamIgnoreAttribute))))
                {
                    PropertyList.Add(prop.Name);
                    /*if (prop.CustomAttributes.Any(x=>x.AttributeType == typeof(DBISamAutoIncAttribute)))
                    {
                        ValueList.Add($"LastAutoInc('{Table}') + 1");
                    }
                    else*/ if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DBISamDateAttribute)))
                        ValueList.Add($"CAST('{((DateTime)prop.GetValue(obj, null)).ToString("yyyy-MM-dd")}' AS DATE)");
                    else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DBISamTimeAttribute)))
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

            if (InsertViaSelect)
                return $"INSERT INTO {Table} ({String.Join(",",PropertyList)}) SELECT {string.Join(",",ValueList)} FROM {Table};";
            else
                return $"INSERT INTO {Table} ({String.Join(",", PropertyList)}) VALUES ({string.Join(",", ValueList)});";
        }
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long ConvertToTimestamp (this DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
    }
    public class DBISamIgnoreAttribute : Attribute
    {
        public bool Ignore { get; }
        public DBISamIgnoreAttribute ()
        {
            this.Ignore = true;
        }
    }
    public class DBISamAutoIncAttribute:Attribute
    {
        public bool AutoInc { get; }
        public DBISamAutoIncAttribute()
        {
            this.AutoInc = true;
        }
    }
    public class DBISamInsertViaSelectAttribute:Attribute
    {
        public bool InsertViaSelect { get; }
        public DBISamInsertViaSelectAttribute()
        {
            this.InsertViaSelect = true;
        }
    }
    public class DBISamDateAttribute:Attribute
    {
        public bool Date { get; }

        public DBISamDateAttribute()
        {
            Date = true;
        }
    }
    public class DBISamTimeAttribute : Attribute
    {
        public bool Time { get; }

        public DBISamTimeAttribute()
        {
            Time = true;
        }
    }
    public class DBISamTableAttribute : Attribute
    {
        public string TableName { get; }
        public DBISamTableAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }
}
