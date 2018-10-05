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
           /// <typeparam name="T"></typeparam>
           /// <param name="Obj"></param>
           /// <returns></returns>
            public virtual async Task<int> Create<T>(T Obj)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand{
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query
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
                                return Convert.ToInt32(command.Parameters["@Id"]);
                            }
                            throw new Exception("Error connecting to data source");
                        }
                        catch (Exception ex) {
                            throw ex;
                        }
                    }
                }
            }
            /// <summary>
            /// removes a record from the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public virtual async Task Delete<T>(T Obj)
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query
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
            /// <returns></returns>
            public async Task<List<T>> ReadAll<T>() where T : new()
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure
                    })
                    {
                        try
                        {
                            List<T> dest = new List<T>();
                            await connection.OpenAsync();
                            var result = await command.ExecuteReaderAsync();
                            while(result.Read())
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
            /// <typeparam name="T"></typeparam>
            /// <param name="Id"></param>
            /// <returns></returns>
            public virtual async Task<T> Read<T>(int Id) where T : new()
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString)) {
                    using (SqlCommand command = new SqlCommand {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure
                    }) {
                        try {
                            await connection.OpenAsync();
                            command.Parameters.Add(new SqlParameter("@Id",Id));
                            var result = await command.ExecuteReaderAsync();
                            return SqlHelper.TranslateResults<T>(result);
                        }
                        catch (Exception ex){
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
            public virtual async Task Update<T>(T Obj) 
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query
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
                    if (Obj != null)
                    {
                        Type objectType = Obj.GetType();
                        MemberInfo[] memberinfo = objectType.GetMembers();
                        foreach (MemberInfo m in memberinfo)
                        {
                            if ((m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property) && m.Name != "Id" && m.Name != "Changer")
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
