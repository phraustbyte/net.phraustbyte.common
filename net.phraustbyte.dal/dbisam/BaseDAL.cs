using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace net.phraustbyte.dal
{
    namespace dbisam
    {
        /// <summary>
        /// Represents a connection to a data source
        /// </summary>
        public class BaseDAL : IBaseDAL
        {
            /// <summary>
            /// represents the connection string
            /// </summary>
            public string ConnectionString { get; }
            /// <summary>
            /// represents the query to be executed
            /// </summary>
            public string Query { get; set; }
            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="connectionString"></param>
            public BaseDAL(string connectionString)
            {
                ConnectionString = connectionString;
            }
            /// <summary>
            /// Gets parameters for use with stored procedures
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>
            public List<IDataParameter> GetParameters<T>(T Obj)
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// Creates a record in the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Obj"></param>
            /// <returns></returns>

            public virtual async Task<int> Create<T>(T Obj)
            {
                try
                {
                this.Query = DBISamHelper.GenerateInsertStatment(Obj);
                return await ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
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
                try
                {
                    this.Query = DBISamHelper.GenerateDeleteStatement(Obj);
                    await ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Updates a record in the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj"></param>
            /// <returns></returns>
            
            public virtual async Task Update<T>(T obj)
            {
                try
                {
                    this.Query = DBISamHelper.GenerateUpdateStatment<T>(obj);
                    await ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Reads all the records in a database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public virtual async Task<List<T>> ReadAll<T>() where T : new()
            {
                var dest = new List<T>();
                try
                {
                    using (OdbcConnection cn = new OdbcConnection(this.ConnectionString))
                    {
                        using (OdbcCommand cmd = new OdbcCommand(this.Query, cn))
                        {
                            cn.Open();
                            using (DbDataReader dr = await cmd.ExecuteReaderAsync())
                            {
                                while (dr.Read())
                                {
                                    dest.Add(SqlHelper.TranslateResults<T>(dr));
                                }
                            }
                        }
                    }
                    return dest;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Reads a single record from the database
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Id"></param>
            /// <returns></returns>
            public virtual async Task<T> Read<T>(int Id) where T : new()
            {
                try
                {
                    using (OdbcConnection cn = new OdbcConnection(this.ConnectionString))
                    {
                        using (OdbcCommand cmd = new OdbcCommand(this.Query, cn))
                        {
                            cn.Open();
                            using (DbDataReader dr = await cmd.ExecuteReaderAsync())
                            {
                                return SqlHelper.TranslateResults<T>(dr);
                               
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Executes a scalar query
            /// </summary>
            /// <returns></returns>
            private async Task<dynamic> ExecuteScalar()
            {
                try
                {
                    using (OdbcConnection cn = new OdbcConnection(this.ConnectionString))
                    {
                        using (OdbcCommand cmd = new OdbcCommand(this.Query, cn))
                        {
                            cn.Open();
                            var result = await cmd.ExecuteScalarAsync();
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Executes a non-result query
            /// </summary>
            /// <returns></returns>
            private async Task<dynamic> ExecuteNonQuery()
            {
                try
                {
                    using (OdbcConnection cn = new OdbcConnection(this.ConnectionString))
                    {
                        using (OdbcCommand cmd = new OdbcCommand(this.Query, cn))
                        {
                            cn.Open();
                            int result = await cmd.ExecuteNonQueryAsync();
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ERROR[HY000][Elevate Software][DBISAM] DBISAM Engine Error # 9729 Duplicate key found in the index 'Primary' of the table 'CALLPNT'
                    Regex regex = new Regex(@"^(ERROR\[[a-zA-Z0-9]+\]\[Elevate Software\]\[DBISAM\]) ([a-zA-Z ]+)(# )(\d+)(\W.*)$");
                    Match m = regex.Match(ex.Message);
                    if (m.Success)
                    {
                        if (m.Groups[4].Value == "9729")
                            return -1;
                        else
                            throw ex;
                    }
                    throw ex;
                }
                finally
                {
                }
            }
        }
    }
}
