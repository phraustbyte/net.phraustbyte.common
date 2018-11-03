using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DateAttribute)))
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
                        ValueList.Add($"CAST('{((DateTime)prop.GetValue(obj, null)).ToString("yyyy-MM-dd hh:mm:ss")}' AS TIMESTAMP)");*/
                    ValueList.Add(ConvertToString(prop, obj));
                }
            }
            string Query = "";
            if (InsertViaSelect)
                Query =  $"INSERT INTO {Table} ({String.Join(",",PropertyList)}) SELECT {string.Join(",",ValueList)} FROM {Table};";
            else
                Query = $"INSERT INTO {Table} ({String.Join(",", PropertyList)}) VALUES ({string.Join(",", ValueList)});";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            //Query += $" SELECT LASTAUTOINC('{Table}') FROM {Table} TOP 1;";
            return Query;
        }
        /// <summary>
        /// Generates an update statement using reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GenerateUpdateStatment<T>(T source)
        {
            string IdField = "";
            object Id = null;
            //TableAttribute attrib = source.GetType().GetCustomAttributes(typeof(TableAttribute),false).First(x=>x.)
            object[] attribs = source.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            var attr = attribs.First() as TableAttribute;
            string Table = attr.TableName;
            //bool InsertViaSelect = source.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(InsertViaSelectAttribute));
            List<string> UpdateList = new List<string>();
            var propertyList = source.GetType().GetProperties();
            foreach (var prop in propertyList)
            {
                if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(IdentifierAttribute)))
                {
                    IdField = prop.Name;
                    Id = prop.GetValue(source, null);
                }
                else if (!(prop.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreAttribute))))
                {
                    UpdateList.Add($"{prop.Name}={ConvertToString(prop, source)}");
                    //if (prop.GetValue(source, null) != prop.GetValue(dest, null))
                    //{
                        //if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DateAttribute)))
                        //    UpdateList.Add($"{prop.Name}=CAST('{((DateTime)prop.GetValue(source, null)).ToString("yyyy-MM-dd")}' AS DATE)");
                        //else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(TimeAttribute)))
                        //    UpdateList.Add($"{prop.Name}=CAST('{((TimeSpan)prop.GetValue(source, null)).ToString("c")}' AS TIME)");
                        //else if (prop.PropertyType == typeof(string))
                        //{
                        //    var str = ((string)prop.GetValue(source, null)) ?? "";
                        //    str = str.Replace("'", "'+#39+'");
                        //    UpdateList.Add($"{prop.Name}='{str}'");
                        //}
                        //else if (prop.PropertyType == typeof(int))
                        //    UpdateList.Add($"{prop.Name}={prop.GetValue(source, null).ToString()}");
                        //else if (prop.PropertyType == typeof(float))
                        //    UpdateList.Add($"{prop.Name}={((float)prop.GetValue(source, null)).ToString()}");
                        //else if (prop.PropertyType == typeof(bool))
                        //    UpdateList.Add($"{prop.Name}={(((bool)prop.GetValue(source, null)) ? "True" : "False")}");
                        //else if (prop.PropertyType == typeof(DateTime))
                        //    UpdateList.Add($"{prop.Name}=CAST('{((DateTime)prop.GetValue(source, null)).ToString("yyyy-MM-dd hh:mm:ss")}' AS TIMESTAMP)");
                    //}
                }
            }
            string Query = "";
            Query = $"UPDATE {Table} SET {String.Join(",", UpdateList)} WHERE {IdField} = {ConvertToString(propertyList.First(prop => prop.CustomAttributes.Any(x=>x.AttributeType == typeof(IdentifierAttribute))),source)};";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            return Query;
        }
        /// <summary>
        /// Generates a delete statement using reflection if the object contains no Active flag. 
        /// Existance of an Active flag will change that value to false using an update statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GenerateDeleteStatement<T>(T source)
        {
            string IdField = "";
            object Id = null;
            string ActiveField = "";
            //TableAttribute attrib = source.GetType().GetCustomAttributes(typeof(TableAttribute),false).First(x=>x.)
            object[] attribs = source.GetType().GetCustomAttributes(typeof(TableAttribute), true);
            var attr = attribs.First() as TableAttribute;
            string Table = attr.TableName;
            //bool InsertViaSelect = source.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(InsertViaSelectAttribute));
            List<string> UpdateList = new List<string>();
            var propertyList = source.GetType().GetProperties();
            foreach (var prop in propertyList)
            {
                if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(IdentifierAttribute)))
                {
                    IdField = prop.Name;
                    Id = prop.GetValue(source, null);
                }
                else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(ActiveAttribute)))
                {
                    ActiveField = prop.Name;
                }
            }
            string Query = "";
            if (String.IsNullOrEmpty(ActiveField))
                Query = $"DELETE FROM {Table} WHERE {IdField} = {ConvertToString(propertyList.First(prop => prop.CustomAttributes.Any(x => x.AttributeType == typeof(IdentifierAttribute))), source)}";
            else
                Query = $"UPDATE {Table} SET {ActiveField} = False WHERE {IdField} = {ConvertToString(propertyList.First(prop => prop.CustomAttributes.Any(x => x.AttributeType == typeof(IdentifierAttribute))), source)};";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            return Query;
        }
        /// <summary>
        /// Generates a read statement using an Id. Source object must have a property containing the Identifier attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GenerateReadStatement<T>(object Id)
        {
            string IdField = "";
            object[] attribs = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
            var attr = attribs.First() as TableAttribute;
            string Table = attr.TableName;
            List<string> UpdateList = new List<string>();
            var propertyList = typeof(T).GetProperties();
            foreach (var prop in propertyList)
            {
                if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(IdentifierAttribute)))
                {
                    IdField = prop.Name;
                }
            }
            string Query = "";
            Query = $"SELECT * FROM {Table} WHERE {IdField} = {Id};";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            return Query;
        }
        /// <summary>
        /// Generates a read all statement using reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        
       
        public static string GenerateReadAllStatement<T>()
        {
            object[] attribs = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
            var attr = attribs.First() as TableAttribute;
            string Table = attr.TableName;
            string Query = "";
            Query = $"SELECT * FROM {Table};";
            Query = Query.Replace("\r\n", "'+#13+#10+'");
            return Query;
        }

        private static string ConvertToString(PropertyInfo prop, object source)
        {
            if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(DateAttribute)))
                return $"CAST('{((DateTime)prop.GetValue(source, null)).ToString("yyyy-MM-dd")}' AS DATE)";
            else if (prop.CustomAttributes.Any(x => x.AttributeType == typeof(TimeAttribute)))
                return $"CAST('{((TimeSpan)prop.GetValue(source, null)).ToString("c")}' AS TIME)";
            else if (prop.PropertyType == typeof(string))
            {
                var str = ((string)prop.GetValue(source, null)) ?? "";
                str = str.Replace("'", "'+#39+'");
                return $"'{str}'";
            }
            else if (prop.PropertyType == typeof(int))
                return $"{prop.GetValue(source, null).ToString()}";
            else if (prop.PropertyType == typeof(float))
                return $"{((float)prop.GetValue(source, null)).ToString()}";
            else if (prop.PropertyType == typeof(bool))
                return $"{(((bool)prop.GetValue(source, null)) ? "True" : "False")}";
            else if (prop.PropertyType == typeof(DateTime))
                return $"CAST('{((DateTime)prop.GetValue(source, null)).ToString("yyyy-MM-dd hh:mm:ss")}' AS TIMESTAMP)";
            else
                return null;
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long ConvertToTimestamp (this DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
    }
}
