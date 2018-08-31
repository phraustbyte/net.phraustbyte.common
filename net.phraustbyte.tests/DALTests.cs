using System;
using System.Collections.Generic;
using net.phraustbyte.bll;
using net.phraustbyte.dal;
using net.phraustbyte.dal.mssql;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using Moq;

namespace net.phraustbyte.tests
{
    public class DALTests
    {
        [Fact]
        public async Task MockDAL_Create_Success()
        {
            TestClass tc = new TestClass(GetMock().Object)
            {
                Id = 5,
                CreateUser = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            Assert.Equal(5, await tc.Create());
        }
        [Fact]
        public async Task MocDAL_Read_Success()
        {
            var mock = GetMock();
            TestClass tc = new TestClass(mock.Object)
            {
                Id = 5,
                CreateUser = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            var result = await tc.Read(5);
            Assert.Equal(tc, result);
        }
        [Fact]
        public async Task MocDAL_ReadAll_Success()
        {
            var mock = GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAll();
            Assert.True(result.Count == 3);
        }
        private Mock<IBaseDAL> GetMock()
        {
            var mock =  new Mock<IBaseDAL>();
            mock.Setup(x => x.Create<TestClass>(It.IsAny<TestClass>()))
                .ReturnsAsync(5);
            mock.Setup(x => x.Delete<TestClass>(It.IsAny<TestClass>())).Verifiable();
            mock.Setup(x => x.Update<TestClass>(It.IsAny<TestClass>())).Verifiable();
            mock.Setup(x => x.Read<TestClass>(5))
                .ReturnsAsync(new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, CreateUser = "User", Name = "Name" });
            mock.Setup(x => x.ReadAll<TestClass>())
                .ReturnsAsync(new List<TestClass>
                {
                    new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, CreateUser = "UserFive", Name = "NameFive" },
                    new TestClass { Id = 6, CreatedDate = DateTime.UtcNow, CreateUser = "UserSix", Name = "NameSix" },
                    new TestClass { Id = 7, CreatedDate = DateTime.UtcNow, CreateUser = "UserSeven", Name = "NameSeven" }
                });
            return mock;
                
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
        public TestClass (IBaseDAL dal)
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

        public async Task<List<IBaseBLL>> ReadAll()
        {
            var result = await DataLayer.ReadAll<TestClass>();
            List<IBaseBLL> res = result.ToList<IBaseBLL>();
            return res;
        }

        public async Task<IBaseBLL> Read(int Id)
        {
            return await DataLayer.Read<TestClass>(Id);
        }

        public async Task Update()
        {
            await DataLayer.Update(this);
        }

        public bool Equals(IBaseBLL other)
        {
            if (other is null) return false;
            return (this.Id == other.Id);
        }
    }
}