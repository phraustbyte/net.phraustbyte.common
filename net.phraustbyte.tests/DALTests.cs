using Moq;
using net.phraustbyte.dal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace net.phraustbyte.tests
{
    public class DALTests
    {
       
        [Fact]
        public async Task MockDAL_CreateByGuid_Success()
        {
            TestClass tc = new TestClass(TestConfig.GetMock().Object)
            {
                Id = Guid.NewGuid(),
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            Assert.IsType<Guid>(await tc.Create<Guid>());
        }
        
        [Fact]
        public async Task MockDAL_ReadByGuid_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object)
            {
                Id = Guid.NewGuid(),
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            var result = await tc.Read<TestClass>(Guid.NewGuid());
            Assert.NotEqual(new Guid(), result.Id);
            Assert.Equal(tc.Changer, result.Changer);
            Assert.Equal(tc.Name, result.Name);
        }
        [Fact]
        public async Task MockDAL_ReadAll_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAll<TestClass>();
            Assert.True(result.Count == 3);
        }
        [Fact]
        public async Task MockDAL_ReadAllByFilter_Success()
        {
            var mock = TestConfig.GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAllByFilter<TestClass,object>(new object(),"Changer");
            Assert.True(result.Count == 3);
        }

    }
}
