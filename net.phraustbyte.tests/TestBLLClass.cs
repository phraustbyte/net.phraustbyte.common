using net.phraustbyte.bll;
using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace net.phraustbyte.tests
{
    public class TestBLLClass : BaseBLL
    {
        public string Name { get; set; }
        public string Description { get; set; }
        protected override IBaseDAL DAL { get; }

        public TestBLLClass(IBaseDAL dal)
        {
            DAL = dal;
        }

        public TestBLLClass()
        {
            DAL = null;
        }

        public async override Task<T> Create<T>()
        {
            return await DAL.Create<TestBLLClass,T>(this);
        }

        public async override Task Delete()
        {
            await DAL.Delete(this);
        }

        public async override Task<TOut> Read<TIn, TOut>(TIn Id)
        {
            return await DAL.Read<TIn, TOut>(Id);
        }

        public async override Task<List<T>> ReadAll<T>()
        {
            return await DAL.ReadAll<T>();
        }

        public async override Task Update()
        {
            await DAL.Update(this);
        }

  
        public async override Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey)
        {
            return await DAL.ReadAllByFilter<TOut, TParam>(FilterValue, FilterKey);
        }

    }
}
