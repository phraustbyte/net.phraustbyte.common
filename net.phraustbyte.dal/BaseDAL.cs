using System;
using System.Collections.Generic;

namespace net.phraustbyte.dal
{
    namespace mssql
    {
        using System.Data;
        using System.Data.SqlClient;
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
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand{
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = this.Query
                    }) 
                    {
                        try
                        {
                            await connection.OpenAsync();
                            command.Parameters.AddRange(GetParameters(Obj).ToArray());
                            var result = await command.ExecuteNonQueryAsync();
                            return Convert.ToInt32(command.Parameters["@Id"]);
                        }
                        catch (Exception ex) {
                            throw ex;
                        }
                    }
                }
            }

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

            public async Task<List<T>> List<T>() where T : new()
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
                                dest.Add(TranslateResults<T>(result));
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
                using (SqlConnection connection = new SqlConnection(this.ConnectionString)) {
                    using (SqlCommand command = new SqlCommand {
                        CommandText = this.Query,
                        CommandType = System.Data.CommandType.StoredProcedure
                    }) {
                        try {
                            await connection.OpenAsync();
                            command.Parameters.Add(new SqlParameter("@Id",Id));
                            var result = await command.ExecuteReaderAsync();
                            return TranslateResults<T>(result);
                        }
                        catch (Exception ex){
                            throw ex;
                        }
                    }
                }
            }

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
            public List<IDataParameter> GetParameters<T>(T Obj) {
                return new List<IDataParameter>();
            }

            private T TranslateResults<T>(IDataReader source) where T:new()
            {
                
                return default(T);
            }
        }
    }
}
