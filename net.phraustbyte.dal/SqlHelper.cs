using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace net.phraustbyte.dal
{
    /// <summary>
    /// Helper Classes for the DAL
    /// </summary>
    static class SqlHelper
    {
        /// <summary>
        /// Dictionary of SQL DB Types
        /// </summary>
        private static Dictionary<Type, SqlDbType> typeMap;

        /// <summary>
        /// Create and populate the dictionary in the static constructor
        /// </summary>
        static SqlHelper()
        {
            typeMap = new Dictionary<Type, SqlDbType>
            {
                [typeof(string)] = SqlDbType.NVarChar,
                [typeof(char[])] = SqlDbType.NVarChar,
                [typeof(byte)] = SqlDbType.TinyInt,
                [typeof(short)] = SqlDbType.SmallInt,
                [typeof(int)] = SqlDbType.Int,
                [typeof(long)] = SqlDbType.BigInt,
                [typeof(byte[])] = SqlDbType.VarBinary,
                [typeof(bool)] = SqlDbType.Bit,
                [typeof(DateTime)] = SqlDbType.DateTime2,
                [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
                [typeof(decimal)] = SqlDbType.Money,
                [typeof(float)] = SqlDbType.Real,
                [typeof(double)] = SqlDbType.Float,
                [typeof(TimeSpan)] = SqlDbType.Time
            };
        }

        /// <summary>
        /// Gets equivelant SQLDataType from specified Type
        /// </summary>
        /// <param name="giveType"></param>
        /// <returns>SqlDataType</returns>
        public static SqlDbType GetDbType(this Type giveType)
        {
            // Allow nullable types to be handled
            giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

            if (typeMap.ContainsKey(giveType))
            {
                return typeMap[giveType];
            }

            throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
        }

        /// <summary>
        /// Gets SqlDBType from Type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>SQLDataType</returns>
        public static SqlDbType GetDbType<T>()
        {
            return GetDbType(typeof(T));
        }
        /// <summary>
        /// Translates an IDataReader objet into an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T TranslateResults<T>(IDataReader source) where T : new()
        {

            if (source == null)
                throw new ArgumentNullException();
            try
            {
                Type objectType = typeof(T);
                //MemberInfo[] memberinfo = objectType.GetMembers();
                var dest = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertyInfo = objectType.GetProperties();
                foreach (var p in propertyInfo)
                {
                    if (p.SetMethod != null)
                    {
                        var drValue = source[p.Name];
                        if (drValue.GetType() == typeof(DBNull))
                            p.SetValue(dest, null, null);
                        else
                        {
                            Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                            var drValueConverted = (drValue == null) ? null : Convert.ChangeType(drValue, t);
                            p.SetValue(dest, drValueConverted, null);
                        }
                    }
                }
                return dest;
            }
            catch (MissingMethodException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    
}
