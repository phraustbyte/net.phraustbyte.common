using System;
using System.Collections.Generic;
using System.Data;

namespace net.phraustbyte.dal
{
    public interface IBaseDAL
    {
        async Task<int> Create<T>(T Obj);
        async Task Update<T>(T Obj);
        async Task Delete<T>(T Obj);
        async Task<List<T>> List<T>() where T :new();
        async Task<T> Read<T>(int Id) where T :new();
        string ConnectionString { get; }
        string Query { get; set; }
        List<IDataParameter> GetParameters<T>(T Obj);
    }
}
