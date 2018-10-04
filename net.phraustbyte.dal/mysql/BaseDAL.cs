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

        public class BaseDAL : IBaseDAL
        {
            public BaseDAL(string ConnectionString)
            {
                this.ConnectionString = ConnectionString ?? throw new ArgumentNullException();
            }

            public string Query { get; set; }

            public string ConnectionString { get; }

            public virtual async Task<int> Create<T>(T Obj)
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
                            return Convert.ToInt32(command.Parameters["@Id"]);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

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
                                dest.Add(TranslateResults<T>(result));
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

            public virtual async Task<T> Read<T>(int Id) where T : new()
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
                            return TranslateResults<T>(result);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

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

            private T TranslateResults<T>(IDataReader source) where T : new()
            {

                if (source == null)
                {
                    throw new ArgumentNullException();
                }

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
                            Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                            var drValueConverted = (drValue == null) ? null : Convert.ChangeType(drValue, t);
                            p.SetValue(dest, drValueConverted, null);
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

            List<IDataParameter> IBaseDAL.GetParameters<T>(T Obj)
            {
                throw new NotImplementedException();
            }
        }

    }
}
