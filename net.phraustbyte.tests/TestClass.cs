﻿using net.phraustbyte.bll;
using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace net.phraustbyte.tests
{
    public class TestClass : IBaseBLL
    {
        public int Id { get; set; }
        public Guid Adjunct { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Changer { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public IBaseDAL DataLayer { get; }

        public TestClass()
        {
            DataLayer = null;// TestConfig.GetMock().Object;  //new BaseDAL("ConnectionString");
        }
        public TestClass(IBaseDAL dal)
        {
            DataLayer = dal;
        }

        public async Task<T> Create<T>()
        {
            return await DataLayer.Create<TestClass,T>(this);
        }
        //public async Task<Guid> Insert()
        //{
        //    return await DataLayer.Insert<Guid,TestClass>(this);
        //}
        public async Task Delete()
        {
            await DataLayer.Delete(this);
        }

        public async Task<List<T>> ReadAll<T>() where T : IBaseBLL, new()
        {
            var res = await DataLayer.ReadAll<T>();
            //List<T> res = result.ToList<IBaseBLL>();
            return res;
        }
        //public async Task<List<TestClass>> IBaseBLL.ReadAll()
        //{
        //    var res = await DataLayer.ReadAll<TestClass>();
        //    return res;
        //}

        public async Task<TOut> Read<TIn,TOut>(TIn Id) where TOut : IBaseBLL, new()
        {
            return await DataLayer.Read<TIn,TOut>(Id);
        }

        public async Task Update()
        {
            await DataLayer.Update(this);
        }

        public bool Equals(IBaseBLL other)
        {
            if (other is null)
            {
                return false;
            }

            return (this.Id == other.Id);
        }

        public async Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey) where TOut : IBaseBLL, new()
        {
           return await DataLayer.ReadAllByFilter<TOut,TParam>(FilterValue, FilterKey);
        }
    }
}
