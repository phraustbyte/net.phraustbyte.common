using System;
using System.Collections.Generic;

namespace net.phraustbyte.dal
{
    namespace mysql
    {
        using MySql.Data;
        using MySql.Data.MySqlClient;
        using System.Data;
        using System.Linq;
        using System.Reflection;
        using System.Threading.Tasks;
        /// <summary>
        /// Represents a connection to connection to a MySQL database
        /// </summary>
        public class BaseDAL : IBaseDAL
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ConnectionString"></param>
            public BaseDAL(string ConnectionString)
            {
                this.ConnectionString = ConnectionString ?? throw new ArgumentNullException();
            }
            /// <summary>
            /// represents a query command or the name of a stored procedure
            /// </summary>
            public string Query { get; set; }
            /// <summary>
            /// represents a connection string to a datasource
            /// </summary>
            public string ConnectionString { get; }

            /// <summary>
            /// Creates a record in a database
            /// </summary>
            /// <typeparam name="TIn"></typeparam>
            /// <typeparam name="TOut"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public virtual async Task<Guid> Create<T>(T Obj)
            {
                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
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
                            return (Guid)command.Parameters["@Id"].Value;

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// removes a record from a database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public virtual async Task Delete<T>(T Obj)
            {
                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
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
            /// Reads all records in a database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public virtual async Task<List<T>> ReadAll<T>() where T : new()
            {
                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
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
                            {
                                dest.Add(SqlHelper.TranslateResults<T>(result));
                            }

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
            /// <typeparam name="T"></typeparam>
            /// <param name="Id"></param>
            /// <returns></returns>
            public virtual async Task<T> Read<T>(Guid Id) where T : new()
            {
                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
                    {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = connection
                    })
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.Add(new MySqlParameter("@Id", Id));
                            var result = await command.ExecuteReaderAsync();
                            return SqlHelper.TranslateResults<T>(result);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// updates a record in a database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public virtual async Task Update<T>(T Obj)
            {
                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
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
            /// Generates a list of parameters based on an object
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
                    Guid? Id = propertyInfo.FirstOrDefault(x => x.Name == "Id").GetValue(Obj, null) as Guid?;
                    string Changer = propertyInfo.FirstOrDefault(x => x.Name == "Changer").GetValue(Obj, null) as string;
                    List<IDataParameter> Params = new List<IDataParameter>();

                    //if (Id == 0)
                    //{
                    //    Params.Add(new SqlParameter("@Id", SqlDbType.Int)
                    //    {
                    //        Direction = System.Data.ParameterDirection.Output
                    //    });
                    //}
                    //else if (Id > 0)
                    //{
                    //    Params.Add(new SqlParameter("@Id", SqlDbType.Int)
                    //    {
                    //        Value = Id
                    //    });
                    //}

                    if (Id == new Guid())
                        Params.Add(new MySqlParameter("@Id", SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                    else
                        Params.Add(new MySqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = Id });


                    //    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal))
                    //    {
                    //        var Adjunct = Convert.ChangeType(property.GetValue(Obj, null), property.PropertyType);
                    //        var defValue = Activator.CreateInstance(property.PropertyType);
                    //        if (Adjunct == defValue)
                    //            Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Direction = System.Data.ParameterDirection.Output });
                    //        else
                    //            Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Value = Adjunct });
                    //    }
                    //    else if (property.PropertyType == typeof(Guid))
                    //    {
                    //        var Adjunct = (Guid)property.GetValue(Obj, null);
                    //        if (Adjunct == new Guid())
                    //            Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Direction = System.Data.ParameterDirection.Output });
                    //        else
                    //            Params.Add(new SqlParameter("@Adjunct", SqlHelper.GetDbType(property.PropertyType)) { Value = Adjunct });
                    //    }
                    //}

                    if (Obj != null)
                    {
                        Type objectType = Obj.GetType();
                        MemberInfo[] memberinfo = objectType.GetMembers();
                        foreach (MemberInfo m in memberinfo)
                        {
                            if ((m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property) && m.Name != "Id" && m.Name != "Changer")
                            {
                                Params.Add(new MySqlParameter("@" + m.Name, SqlHelper.GetDbType(Obj.GetType().GetProperty(m.Name).PropertyType))
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
                        Params.Add(new MySqlParameter("@Changer", Changer));
                    }
                    return Params;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating SQL Parameters", ex);
                }
            }
            public virtual async Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey) where TOut : new()
            {
                var objName = typeof(TOut).Name;
                this.Query = $"App.usp{objName}_SelectAllByFilter";

                using (MySqlConnection connection = new MySqlConnection(this.ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand
                    {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        Connection = connection
                    })
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.Add(new MySqlParameter($"@{FilterKey}", SqlHelper.GetDbType<TParam>())
                            {
                                Value = FilterValue
                            });
                            var reader = await command.ExecuteReaderAsync();
                            if (reader.HasRows)
                            {
                                List<TOut> dest = new List<TOut>();
                                await connection.OpenAsync();
                                var result = await command.ExecuteReaderAsync();
                                while (result.Read())
                                    dest.Add(SqlHelper.TranslateResults<TOut>(result));
                                return dest;
                            }
                            else
                            {
                                throw new RecordNotFoundException($"Filter Value: {FilterValue.ToString()}");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
