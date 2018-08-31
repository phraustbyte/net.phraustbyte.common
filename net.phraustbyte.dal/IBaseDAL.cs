using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace net.phraustbyte.dal
{
    public interface IBaseDAL
    {
        Task<int> Create<T>(T Obj);
        Task Update<T>(T Obj);
        Task Delete<T>(T Obj);
        Task<List<T>> ReadAll<T>() where T :new();
        Task<T> Read<T>(int Id) where T :new();
        string ConnectionString { get; }
        string Query { get; set; }
        List<IDataParameter> GetParameters<T>(T Obj);
    }
}
