using Moq;
using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace net.phraustbyte.tests
{
    public class DALTests
    {
        [Fact]
        public async Task MockDAL_CreateByInt_Success()
        {
            TestClass tc = new TestClass(TestConfig.GetMock().Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            Assert.Equal(5, await tc.Create<int>());
        }
        [Fact]
        public async Task MockDAL_CreateByGuid_Success()
        {
            TestClass tc = new TestClass(TestConfig.GetMock().Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            Assert.IsType<Guid>(await tc.Create<Guid>());
        }
        //[Fact]
        //public async Task MockDAL_Insert_Success()
        //{
        //    TestClass tc = new TestClass(GetMock().Object)
        //    {
        //        Id = 5,
        //        Changer = "User",
        //        Name = "Name",
        //        CreatedDate = DateTime.UtcNow
        //    };
        //    var result = await tc.Insert();
        //    Assert.IsType<Guid>(result);
        //}
        [Fact]
        public async Task MocDAL_ReadById_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            var result = await tc.Read<int,TestClass>(5);
            Assert.Equal(tc.Id, result.Id);
            Assert.Equal(tc.Changer, result.Changer);
            Assert.Equal(tc.Name, result.Name);
        }
        [Fact]
        public async Task MocDAL_ReadByGuid_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow,
                Adjunct = new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8")
            };
            var result = await tc.Read<Guid,TestClass>(new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"));
            Assert.Equal(tc.Id, result.Id);
            Assert.Equal(tc.Changer, result.Changer);
            Assert.Equal(tc.Name, result.Name);
        }
        [Fact]
        public async Task MocDAL_ReadAll_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAll<TestClass>();
            Assert.True(result.Count == 3);
        }
        [Fact]
        public async Task MocDAL_ReadAllByFilter_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAllByFilter<TestClass,object>(new object(),"Changer");
            Assert.True(result.Count == 3);
        }

    }
}
