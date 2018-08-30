using System;
using System.Collections.Generic;
using net.phraustbyte.bll;
using net.phraustbyte.dal;
using net.phraustbyte.dal.mssql;
using Xunit;

namespace net.phraustbyte.tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }

    public class TestClass : IBaseBLL
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string Name { get; set; }
        private IBaseDAL DataLayer { get; set; }

        public TestClass()
        {
            DataLayer = new BaseDAL("ConnectionString");
        }

        public int Create()
        {
            DataLayer.Parameters = DataLayer.GetParameters<DataLayer.ParameterType,TestClass>(this);
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public List<IBaseBLL> List()
        {
            throw new NotImplementedException();
        }

        public IBaseBLL Read(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
