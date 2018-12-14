using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace net.phraustbyte.dal
{
    /// <summary>
    /// Represents a connection to a data source
    /// </summary>
    public interface IBaseDAL
    {
        /// <summary>
        /// Creates a record in a database
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        Task<TOut> Create<TIn,TOut>(TIn Obj);

        /// <summary>
        /// Updates a record in a database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        Task Update<T>(T Obj);
        /// <summary>
        /// Deletes a record in a database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        Task Delete<T>(T Obj);
        /// <summary>
        /// Reads all records in a database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> ReadAll<T>() where T :new();
        /// <summary>
        /// Reads a single record in a database
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TOut> Read<TIn,TOut>(TIn Id) where TOut :new();
        /// <summary>
        /// Represents a connection string to a datasource
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Represents a query command or name of a stored procedure
        /// </summary>
        string Query { get; set; }
        /// <summary>
        /// Generates a list of parameters based on an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        List<IDataParameter> GetParameters<T>(T Obj);
    }
}
