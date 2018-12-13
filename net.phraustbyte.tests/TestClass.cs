using net.phraustbyte.bll;
using net.phraustbyte.dal;
using net.phraustbyte.dal.mssql;
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
            DataLayer = new BaseDAL("ConnectionString");
        }
        public TestClass(IBaseDAL dal)
        {
            DataLayer = dal;
        }

        public async Task<int> Create()
        {
            return await DataLayer.Create(this);
        }

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

        public async Task<T> Read<T>(int Id) where T : IBaseBLL, new()
        {
            return await DataLayer.Read<T>(Id);
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
    }
}
