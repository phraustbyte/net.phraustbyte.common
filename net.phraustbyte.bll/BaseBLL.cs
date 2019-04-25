using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace net.phraustbyte.bll
{
    /// <summary>
    /// Represents the base BLL object
    /// </summary>
    public abstract class BaseBLL : IBaseBLL
    {
        /// <summary>
        /// Integer Identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Date/Time the record was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Changer of the record
        /// </summary>
        public string Changer { get; set; }
        /// <summary>
        /// Active Flag
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// represents the DAL
        /// </summary>
        protected abstract IBaseDAL DAL {get;}

        /// <summary>
        /// Creates a record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract Task<Guid> Create<T>();
        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <returns></returns>
        public abstract Task Delete();
        /// <summary>
        /// Reads a record by identifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public abstract Task<T> Read<T>(Guid Id) where T : IBaseBLL, new();
        /// <summary>
        /// Reads all records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract Task<List<T>> ReadAll<T>() where T : IBaseBLL, new();
        /// <summary>
        /// Updates a record
        /// </summary>
        /// <returns></returns>
        public abstract Task Update();
        /// <summary>
        /// Reads all records by a filter
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="FilterValue"></param>
        /// <param name="FilterKey"></param>
        /// <returns></returns>
        public abstract Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey) where TOut : new();
        
    }
}
