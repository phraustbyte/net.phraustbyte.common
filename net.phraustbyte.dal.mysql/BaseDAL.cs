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
            public virtual async Task<TOut> Create<TIn,TOut>(TIn Obj)
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
                            return (TOut)command.Parameters[typeof(TOut) == typeof(int) ? "@Id" : "@Adjunct"].Value;

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
            public virtual async Task<TOut> Read<TIn,TOut>(TIn Id) where TOut : new()
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
                            command.Parameters.Add(new MySqlParameter(
                                typeof(TIn) == typeof(int) ? "@Id" : "@Adjunct", Id));
                            var result = await command.ExecuteReaderAsync();
                            return SqlHelper.TranslateResults<TOut>(result);
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
                    int? Id = propertyInfo.FirstOrDefault(x => x.Name == "Id").GetValue(Obj, null) as int?;
                    string Changer = propertyInfo.FirstOrDefault(x => x.Name == "Changer").GetValue(Obj, null) as string;

                    List<IDataParameter> Params = new List<IDataParameter>();
                    if (Id == 0)
                    {
                        Params.Add(new MySqlParameter("@Id", SqlDbType.Int)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        });
                    }
                    else if (Id > 0)
                    {
                        Params.Add(new MySqlParameter("@Id", SqlDbType.Int)
                        {
                            Value = Id
                        });
                    }
                    if (Obj != null)
                    {
                        Type objectType = Obj.GetType();
                        MemberInfo[] memberinfo = objectType.GetMembers();
                        foreach (var p in propertyInfo)
                        {
                            if ((p.MemberType == MemberTypes.Field || p.MemberType == MemberTypes.Property) &&
                                p.Name != "Id" &&
                                p.Name != "Changer" &&
                                !typeof(IBaseDAL).IsAssignableFrom(p.PropertyType))
                            //p.PropertyType == typeof(IBaseDAL))
                            {
                                Params.Add(new MySqlParameter("@" + p.Name, SqlHelper.GetDbType(Obj.GetType().GetProperty(p.Name).PropertyType))
                                {
                                    Value = Obj.GetType().GetProperty(p.Name).GetValue(Obj, null)
                                });
                            }
                        }

                        //MySqlParameter XmlParam = new MySqlParameter("@Object", SqlDbType.Xml);
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
            //private T TranslateResults<T>(IDataReader source) where T : new()
            //{

            //    if (source == null)
            //    {
            //        throw new ArgumentNullException();
            //    }

            //    try
            //    {
            //        Type objectType = typeof(T);
            //        //MemberInfo[] memberinfo = objectType.GetMembers();
            //        var dest = (T)Activator.CreateInstance(typeof(T));
            //        PropertyInfo[] propertyInfo = objectType.GetProperties();
            //        foreach (var p in propertyInfo)
            //        {
            //            if (p.SetMethod != null)
            //            {
            //                var drValue = source[p.Name];
            //                Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            //                var drValueConverted = (drValue == null) ? null : Convert.ChangeType(drValue, t);
            //                p.SetValue(dest, drValueConverted, null);
            //            }
            //        }
            //        return dest;
            //    }
            //    catch (MissingMethodException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (Exception ex)
            //    {

            //        throw ex;
            //    }
            //}

            List<IDataParameter> IBaseDAL.GetParameters<T>(T Obj)
            {
                throw new NotImplementedException();
            }

        }

    }
}
