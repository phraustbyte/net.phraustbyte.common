﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace net.phraustbyte.bll
{
    /// <summary>
    /// Represents the base of an object
    /// </summary>
    public interface IBaseBLL
    {
        /// <summary>
        /// Represents the Id of an object
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Represents the unique identifier of an object
        /// </summary>
        Guid Adjunct { get; set; }
        /// <summary>
        /// Represents the date the record was created
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// Represents the person who changed the record
        /// </summary>
        string Changer { get; set; }
        /// <summary>
        /// Indicates whether the record is active (not deleted)
        /// </summary>
        bool Active { get; set; }
        /// <summary>
        /// Creates a record in the database
        /// </summary>
        /// <returns></returns>
        Task<T> Create<T>();
        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <returns></returns>
        Task Update();
        /// <summary>
        /// Removes a record in the database
        /// </summary>
        /// <returns></returns>
        Task Delete();
        /// <summary>
        /// Reads all records in the database
        /// </summary>
        /// <returns></returns>
        Task<List<T>> ReadAll<T>() where T: IBaseBLL, new();
        /// <summary>
        /// Reads a single record in a database
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TOut> Read<TIn,TOut>(TIn Id) where TOut: IBaseBLL, new();

    }
}
