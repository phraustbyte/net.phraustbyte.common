using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;

namespace net.phraustbyte.dal
{
    namespace dbisam
    {
        class BaseDAL : IBaseDAL
        {
            public string ConnectionString { get; }

            public string Query { get; set; }

            public List<IDataParameter> GetParameters<T>(T Obj)
            {
                throw new NotImplementedException();
            }


            public virtual async Task<int> Create<T>(T Obj)
            {
                this.Query = DBISamHelper.GenerateInsertStatment(Obj);
                return await ExecuteScalar();
               
            }

            public virtual Task Delete<T>(T Obj)
            {
                throw new NotSupportedException();
                //var result = Task.Run(async () => { return await ExecuteQuery(); }).Result;
            }
            public virtual Task Update<T>(T Obj)
            {
                throw new NotSupportedException();
               // var result = Task.Run(async () => { return await ExecuteQuery(); }).Result;
            }

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
            private async Task<dynamic> ExecuteQuery()
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
                    throw ex;
                }
                finally
                {
                }
            }
        }
    }
}
