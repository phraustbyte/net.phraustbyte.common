using System;
using System.Collections.Generic;

namespace net.phraustbyte.dal
{
    namespace mssql
    {
        using System.Data;
        using System.Data.SqlClient;
        using System.Linq;
        using System.Reflection;
        using System.Threading.Tasks;
        /// <summary>
        /// represents a connection to a Microsoft SQL datasource
        /// </summary>
        public class BaseDAL : IBaseDAL
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ConnectionString"></param>
            /// <exception cref="System.ArgumentNullException">Thrown when Connection string is null</exception>
            public BaseDAL(string ConnectionString)
            {
                this.ConnectionString = ConnectionString ?? throw new ArgumentNullException();

            }
            /// <summary>
            /// Represents a query to be executed or the name of a stored procedure
            /// </summary>
            public string Query { get; set; }
            /// <summary>
            /// Represents the connection string to the datasource
            /// </summary>
            public string ConnectionString { get; }
            /// <summary>
            /// Creates a record in the database
            /// </summary>
            /// <typeparam name="TIn"></typeparam>
            /// <typeparam name="TOut"></typeparam>
            /// <param name="Obj"></param>
            /// <returns>Database Record</returns>
            /// <example>
            /// <code>
            /// Object obj = new Object() ;
            /// int recordIndex = await Create&gt;Object&lt;(obj);
            /// </code>
            /// </example>
            public virtual async Task<TOut> Create<TIn, TOut>(TIn Obj)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query,
                        Connection = connection
                    })
                    {
                        try
                        {
                            var q = connection.OpenAsync();
                            q.Wait();
                            if (q.IsCompleted)
                            {
                                command.Parameters.AddRange(GetParameters(Obj).ToArray());
                                var result = await command.ExecuteNonQueryAsync();

                                return (TOut)command.Parameters[typeof(TOut) == typeof(int)?"@Id":"@Adjunct"].Value;
                            }
                            throw new Exception("Error connecting to data source");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            ///// <summary>
            ///// Creates a record in the database
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="Obj"></param>
            ///// <returns>Database Record</returns>
            ///// <example>
            ///// <code>
            ///// Object obj = new Object() ;
            ///// Guid recordIndex = await Create&gt;Object&lt;(obj);
            ///// </code>
            ///// </example>
            //public virtual async Task<Guid> Insert<T>(T Obj)
            //{
            //    using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            //    {
            //        using (SqlCommand command = new SqlCommand
            //        {
            //            CommandType = System.Data.CommandType.StoredProcedure,
            //            CommandText = this.Query,
            //            Connection = connection
            //        })
            //        {
            //            try
            //            {
            //                var q = connection.OpenAsync();
            //                q.Wait();
            //                if (q.IsCompleted)
            //                {
            //                    command.Parameters.AddRange(GetParameters(Obj).ToArray());
            //                    var result = await command.ExecuteNonQueryAsync();
            //                    return (Guid)command.Parameters["@Adjunct"].Value;
            //                }
            //                throw new Exception("Error connecting to data source");
            //            }
            //            catch (Exception ex)
            //            {
            //                throw ex;
            //            }
            //        }
            //    }
            //}

            /// <summary>
            /// removes a record from the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// Object obj = new Object();
            /// await Delete(obj);
            /// </code>
            /// </example>
            public virtual async Task Delete<T>(T Obj)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query,
                        Connection = connection
                    })
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.AddRange(GetParameters(Obj).ToArray());
                            var result = await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// Reads all records from the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns>List of records</returns>
            /// <example>
            /// <code>
            /// List&gt;Object&lt; list = new List&gt;Object&lt;();
            /// list = await ReadAll&gt;Object&lt;();
            /// </code>
            /// </example>
            public virtual async Task<List<T>> ReadAll<T>() where T : new()
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = connection
                    })
                    {
                        try
                        {
                            List<T> dest = new List<T>();
                            await connection.OpenAsync();
                            var result = await command.ExecuteReaderAsync();
                            while (result.Read())
                                dest.Add(SqlHelper.TranslateResults<T>(result));
                            return dest;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// Reads a record from a database
            /// </summary>
            /// <typeparam name="TIn"></typeparam>
            /// <typeparam name="TOut"></typeparam>
            /// <param name="Id"></param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// int id = 1;
            /// Object obj = await Read&gt;Object&lt;(id);
            /// </code>
            /// </example>
            public virtual async Task<TOut> Read<TIn,TOut>(TIn Id) where TOut : new()
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = connection
                    })
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.Add(new SqlParameter(
                                typeof(TIn) == typeof(int)?"@Id":"@Adjunct", Id));
                            var reader = await command.ExecuteReaderAsync();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return SqlHelper.TranslateResults<TOut>(reader);
                            }
                            else
                            {
                                throw new RecordNotFoundException($"Record Id: {Id.ToString()}");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// Updates a record in a database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// RecordObject obj = new RecordObj {
            ///     Id = 1
            /// } ;
            /// await Update&gt;RecordObject&lt;(obj);
            /// </code>
            /// </example>
            public virtual async Task Update<T>(T Obj)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query,
                        Connection = connection
                    })
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.AddRange(GetParameters(Obj).ToArray());
                            var result = await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// Generates parameters based on an object
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public List<IDataParameter> GetParameters<T>(T Obj)
            {

                try
                {
                    PropertyInfo[] propertyInfo = Obj.GetType().GetProperties();
                    propertyInfo.DefaultIfEmpty(null);
                    int? Id = propertyInfo.FirstOrDefault(x => x.Name == "Id").GetValue(Obj, null) as int?;
                    string Changer = propertyInfo.FirstOrDefault(x => x.Name == "Changer").GetValue(Obj, null) as string;
                    List<IDataParameter> Params = new List<IDataParameter>();

                    if (Id == 0)
                    {
                        Params.Add(new SqlParameter("@Id", SqlDbType.Int)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });
                    }
                    else if (Id > 0)
                    {
                        Params.Add(new SqlParameter("@Id", SqlDbType.Int)
                        {
                            Value = Id
                        });
                    }
                    if (propertyInfo.Any(x => x.Name == "Adjunct"))
                    {
                        var property = propertyInfo.FirstOrDefault(x => x.Name == "Adjunct");
                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal))
                        {
                            var Adjunct = Convert.ChangeType(property.GetValue(Obj, null), property.PropertyType);
                            var defValue = Activator.CreateInstance(property.PropertyType);
                            if (Adjunct == defValue)
                                Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Direction = System.Data.ParameterDirection.Output });
                            else
                                Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Value = Adjunct });
                        }
                        else if (property.PropertyType == typeof(Guid))
                        {
                            var Adjunct = (Guid)property.GetValue(Obj, null);
                            if (Adjunct == new Guid())
                                Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Direction = System.Data.ParameterDirection.Output });
                            else
                                Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Value = Adjunct });
                        }
                    }

                    if (Obj != null)
                    {
                        Type objectType = Obj.GetType();
                        MemberInfo[] memberinfo = objectType.GetMembers();
                        foreach (MemberInfo m in memberinfo)
                        {
                            if ((m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property) && m.Name != "Id" && m.Name != "Changer" && m.Name != "Adjunct")
                            {
                                Params.Add(new SqlParameter("@" + m.Name, SqlHelper.GetDbType(Obj.GetType().GetProperty(m.Name).PropertyType))
                                {
                                    Value = Obj.GetType().GetProperty(m.Name).GetValue(Obj, null)
                                });
                            }
                        }

                        //SqlParameter XmlParam = new SqlParameter("@Object", SqlDbType.Xml);
                        //XmlParam.Value = ProcessXML.SerializeObject(obj);
                        //Params.Add(XmlParam);
                    }
                    if (String.IsNullOrEmpty(Changer))
                    {
                        throw new Exception("Changer must contain a value");
                    }
                    else
                    {
                        Params.Add(new SqlParameter("@Changer", Changer));
                    }
                    return Params;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating SQL Parameters", ex);
                }
            }


        }

    }
}
