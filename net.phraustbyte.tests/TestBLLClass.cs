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

        public async override Task<Guid> Create<T>()
        {
            return await DAL.Create(this);
        }

        public async override Task Delete()
        {
            await DAL.Delete(this);
        }

        public async override Task<T> Read<T>(Guid Id)
        {
            return await DAL.Read<T>(Id);
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
